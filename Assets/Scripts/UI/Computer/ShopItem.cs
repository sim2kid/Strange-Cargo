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
        
        private RenderTexture renderTexture;
        public Texture2D texture;

        [SerializeField]
        private GameObject icon;

        private GameObject demoObject;

        private delegate void RenderAction();
        private RenderAction RenderTextureMethod;

        private int resolution = 128;

        private void Start()
        {
            Rect camRect = new Rect(0, 0, resolution, resolution);
            renderTexture = new RenderTexture((int)camRect.width, (int)camRect.height, 24);
            texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);


            SpawnObject(new Vector3(0, -100, 0));
            GameObject cameraObj = Instantiate(new GameObject(), new Vector3(0,-100,-10), Quaternion.identity);
            Camera cam = cameraObj.AddComponent<Camera>();
            cam.farClipPlane = 50f;
            cam.targetTexture = renderTexture;
            cam.rect = camRect;

            RenderTextureMethod = () => 
            {
                cam.Render();
                RenderTexture.active = renderTexture;
                texture.ReadPixels(camRect, 0, 0);

                Destroy(cam.gameObject);
                RenderTexture.active = null;
                Destroy(renderTexture);
                renderTexture = null;
                Destroy(demoObject);

                texture.Apply();
                icon.GetComponent<Renderer>().material.mainTexture = texture;
            };
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
            if (RenderTextureMethod != null) 
            {
                RenderTextureMethod.Invoke();
                RenderTextureMethod = null;
            }
        }
    }
}