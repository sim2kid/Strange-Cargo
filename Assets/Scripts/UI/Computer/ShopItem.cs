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
        
        public RenderTexture texture;

        private GameObject demoObject;

        private delegate void RenderAction();
        private RenderAction RenderTexture;

        private void Start()
        {
            SpawnObject(new Vector3(0, -100, 0));
            GameObject cameraObj = Instantiate(new GameObject(), new Vector3(0,-100,-10), Quaternion.identity);
            Camera cam = cameraObj.AddComponent<Camera>();
            cam.farClipPlane = 50f;
            
        }

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
            if (RenderTexture != null) 
            {
                RenderTexture.Invoke();
            }
        }
    }
}