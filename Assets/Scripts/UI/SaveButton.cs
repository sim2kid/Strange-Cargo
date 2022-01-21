using PersistentData;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class SaveButton : MonoBehaviour
    {
        [SerializeField]
        public SaveMeta meta;

        [SerializeField]
        Image Background;
        [SerializeField]
        Image image;
        [SerializeField]
        TextMeshProUGUI Name;
        [SerializeField]
        TextMeshProUGUI time;
        [SerializeField]
        TextMeshProUGUI Version;

        public bool Selected = false;

        UnityEvent OnAnySelected;

        float lastClickTime;
        [SerializeField]
        float doubleClickSpeed = 0.5f;

        public void SetUp(SaveMeta metaData, UnityEvent onAnySelected) 
        {
            OnAnySelected = onAnySelected;
            OnAnySelected.AddListener(Deselect);
            Name.text = metaData.SaveName;
            var date = (new DateTime(1970, 1, 1)).AddMilliseconds(metaData.SaveTime).ToLocalTime();
            time.text = $"{date.ToString("d")} {date.ToString("t")}";
            Version.text = $"v{metaData.GameVersion}";
            meta = metaData;
        }

        private void OnDestroy()
        {
            OnAnySelected.RemoveListener(Deselect);
        }

        public void Deselect() 
        {
            Background.color = new Color(0, 0, 0, 0);
            Selected = false;
        }

        public void Load() 
        {
            if (Time.realtimeSinceStartup - lastClickTime < doubleClickSpeed)
            {
                SaveManager manager = FindObjectOfType<SaveManager>();
                if (manager == null)
                {
                    Console.LogError("There is no save manager in scene.");
                    return;
                }
                manager.LoadSave(meta.SaveGuid);
            }
            OnAnySelected.Invoke();
            Selected = true;
            Background.color = GetComponent<Button>().colors.highlightedColor;
            lastClickTime = Time.realtimeSinceStartup;
        }
    }
}