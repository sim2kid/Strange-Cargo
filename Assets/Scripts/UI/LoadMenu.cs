using PersistentData;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class LoadMenu : MonoBehaviour
    {
        [SerializeField]
        GameObject SavePrefab;
        [SerializeField]
        RectTransform Content;
        [SerializeField]
        Button PlayButton;
        [SerializeField]
        Button DeleteButton;
        [SerializeField]
        GameObject DefaultButton;
        [SerializeField]
        GameObject DefaultDelete;
        [SerializeField]
        GameObject DefaultNewGameButton;
        [SerializeField]
        GameObject PickSaveMenu;
        [SerializeField]
        GameObject ConfirmDeleteMenu;
        [SerializeField]
        GameObject NewSaveMenu;


        [SerializeField]
        TMPro.TMP_InputField saveName;

        List<SaveButton> SaveButtons;

        SaveManager manager;
        SaveMeta context;
        UnityEvent OnSaveSelected = new UnityEvent();

        void OnEnable()
        {
            ConfirmDeleteMenu.SetActive(false);
            PickSaveMenu.SetActive(true);
            NewSaveMenu.SetActive(false);
            manager = FindObjectOfType<SaveManager>();
            if (manager == null)
            {
                Console.LogError("There is no save manager in scene.");
                return;
            }
            if (SavePrefab == null)
            {
                Console.LogError("No save prefab to build menu out of.");
                return;
            }

            float height = 100;
            List<SaveMeta> metas = manager.GetSaveList();
            float expandBy = (height * 0.15f) + ((height * 0.15f) + (height) * metas.Count);
            metas.Sort((x, y) => y.SaveTime.CompareTo(x.SaveTime));
            Content.sizeDelta = new Vector2(Content.sizeDelta.x, expandBy);
            SaveButtons = new List<SaveButton>();
            for (int i = 0; i < metas.Count; i++)
            {
                SaveMeta meta = metas[i];
                GameObject entry = Instantiate(SavePrefab);
                
                entry.transform.SetParent(Content, false);
                RectTransform entryRect = (RectTransform)entry.transform;
                entryRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, (entryRect.rect.height * 0.15f) + (i * entryRect.rect.height),
                    entryRect.rect.height);
                var btn = entry.GetComponent<SaveButton>();
                btn?.SetUp(meta, OnSaveSelected);
                SaveButtons.Add(btn);
            }
            DeselectButton();
        }



        private void OnDisable()
        {
            OnSaveSelected.RemoveAllListeners();
            if(Content != null)
                if(Content.transform != null)
                    foreach (Transform child in Content.transform)
                    {
                        Destroy(child.gameObject);
                    }
            if(SaveButtons != null)
                SaveButtons.Clear();
            DeselectButton();
            context = new SaveMeta();
        }

        public void Rebake() 
        {
            OnDisable();
            OnEnable();
        }

        void Update() 
        {
            if (SaveButtons == null)
                return;
            foreach (var btn in SaveButtons)
            {
                if (btn.Selected)
                {
                    ButtonSelected(btn.meta);
                    return;
                }
            }
            DeselectButton();
        }

        public void ButtonSelected(SaveMeta metadata)
        {
            PlayButton.interactable = true;
            DeleteButton.interactable = true;
            context = metadata;
        }

        private void DeselectButton()
        {
            PlayButton.interactable = false;
            DeleteButton.interactable = false;
            context = new SaveMeta();
        }

        public void Play()
        {
            string guid = context.SaveGuid;
            if (string.IsNullOrEmpty(guid))
                return;

            manager.LoadSave(context.SaveGuid);
        }

        public void NewGameMenu() 
        {
            // Open save menu
            ConfirmDeleteMenu.SetActive(false);
            PickSaveMenu.SetActive(false);
            NewSaveMenu.SetActive(true);
            if(saveName != null)
                saveName.text = "Unnamed Game";
            EventSystem.current.SetSelectedGameObject(DefaultNewGameButton);
        }

        public void NewGame() 
        {
            // Create new save
            if (saveName != null)
                if (!string.IsNullOrWhiteSpace(saveName.text))
                    manager.StartNewSave(saveName.text);
            BackToLoadMenu();
        }

        public void Delete()
        {
            string guid = context.SaveGuid;
            if (string.IsNullOrEmpty(guid))
                return;
            ConfirmDeleteMenu.SetActive(true);
            PickSaveMenu.SetActive(false);
            NewSaveMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(DefaultDelete);
        }

        public void ConfirmDelete()
        {
            Console.Log($"Deleting World '{context.SaveName}' with guid of {context.SaveGuid}.");
            manager.DeleteSave(context.SaveGuid);
            BackToLoadMenu();
        }

        public void BackToLoadMenu()
        {
            ConfirmDeleteMenu.SetActive(false);
            PickSaveMenu.SetActive(true);
            NewSaveMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(DefaultButton);
            Rebake();
        }
    }
}