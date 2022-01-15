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
        GameObject PickSaveMenu;
        [SerializeField]
        GameObject ConfirmDeleteMenu;


        List<SaveButton> SaveButtons;

        SaveManager manager;
        SaveMeta context;
        UnityEvent OnSaveSelected = new UnityEvent();

        void OnEnable()
        {
            ConfirmDeleteMenu.SetActive(false);
            PickSaveMenu.SetActive(true);
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

            List<SaveMeta> metas = manager.GetSaveList();
            metas.Sort((x, y) => y.SaveTime.CompareTo(x.SaveTime));
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
        
        }

        public void NewGame() 
        {
        
        }

        public void Delete()
        {
            string guid = context.SaveGuid;
            if (string.IsNullOrEmpty(guid))
                return;
            ConfirmDeleteMenu.SetActive(true);
            PickSaveMenu.SetActive(false);
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
            EventSystem.current.SetSelectedGameObject(DefaultButton);
            Rebake();
        }
    }
}