using PersistentData.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Computer
{
    public class ItemPanel : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI Title;
        [SerializeField]
        private TMPro.TextMeshProUGUI Description;
        [SerializeField]
        private UnityEngine.UI.Image Icon;

        private PrefabData itemToSpawn;

        public void UpdatePanel(PrefabData data, Texture2D newIcon, string title, string description) 
        {
            gameObject.SetActive(true);
            itemToSpawn = data;
            Title.text = title;
            Description.text = description; 
            Icon.sprite = Sprite.Create(newIcon, new Rect(0,0,128,128), Vector2.zero);
        }

        void Start() 
        {
            gameObject.SetActive(false);
        }

        public void SpawnItem()
        {
            FindObjectOfType<ItemSpawner>().Spawn(itemToSpawn);
        }
    }
}