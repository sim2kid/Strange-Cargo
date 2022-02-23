using PersistentData.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Computer
{
    public class ShopItem : MonoBehaviour
    {
        [SerializeField]
        public string PrefabDataLocation;
        public PrefabData PrefabData => GetPrefabData();

        private GameObject demoObject;

        public void SpawnObject(Vector3 location) 
        {
            demoObject = PersistentData.SaveManager.LoadPrefab(PrefabData);
            demoObject.transform.position = location;
        }

        private PrefabData GetPrefabData() 
        {
            string json = Resources.Load<TextAsset>(PrefabDataLocation).ToString();
            if(string.IsNullOrEmpty(json))
                return null;
            PrefabData data = Newtonsoft.Json.JsonConvert.DeserializeObject<PrefabData>(json);
            return data;
        }

        private void Update()
        {
            
        }
    }
}