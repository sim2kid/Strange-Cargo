using PersistentData.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Computer
{
    public class ShopItem : MonoBehaviour
    {
        [SerializeField]
        public string PrefabDataLocation;
        private PrefabData _data = null;
        public PrefabData PrefabData 
        {
            get 
            { 
                if (_data == null) 
                {
                    _data = GetPrefabData();
                }
                return _data;
            }
            set { _data = value; }
        }
        
        private RenderTexture renderTexture;
        public Texture2D texture;

        [SerializeField]
        private GameObject icon;

        [SerializeField]
        private float itemPrice = 0.00f;
        public float ItemPrice { get { return itemPrice; } }

        private GameObject demoObject;

        private delegate void RenderAction();
        private RenderAction RenderTextureMethod;

        private int resolution = 128;

        private System.Func<PrefabData, Texture2D, string, string, string, bool> onClick = null;

        private void Start()
        {
            Rect camRect = new Rect(0, 0, resolution, resolution);
            renderTexture = new RenderTexture((int)camRect.width, (int)camRect.height, 24);
            texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);


            SpawnObject(new Vector3(0, -100, 0));
            GameObject cameraObj = Instantiate(new GameObject(), new Vector3(0,-100,-2), Quaternion.identity);
            Camera cam = cameraObj.AddComponent<Camera>();
            cam.farClipPlane = 50f;
            cam.targetTexture = renderTexture;
            cam.rect = camRect;
            demoObject.SetActive(false);

            RenderTextureMethod = () => 
            {
                demoObject.SetActive(true);
                cam.Render();
                RenderTexture.active = renderTexture;
                texture.ReadPixels(camRect, 0, 0);
                demoObject.SetActive(false);

                RenderTexture.active = null;
                renderTexture = null;

                RenderTextureMethod = () =>
                {
                    Destroy(renderTexture);
                    Destroy(cam.gameObject);
                    Destroy(demoObject);
                    RenderTextureMethod = null;

                    this.GetComponent<Button>().onClick.AddListener(() => {
                        if (onClick != null)
                            onClick.Invoke(PrefabData, texture, PrefabData.PrefabResourceLocation, "Demo Item and description", itemPrice.ToString("0.00"));
                    });
                };

                texture.Apply();
                Sprite sprite = Sprite.Create(texture, camRect, Vector2.zero);
                icon.GetComponent<Image>().sprite = sprite;
            };
        }

        public void SpawnObject(Vector3 location) 
        {
            demoObject = PersistentData.SaveManager.LoadPrefab(PrefabData);
            demoObject.transform.position = location;
        }

        private PrefabData GetPrefabData() 
        {
            TextAsset ta = Resources.Load<TextAsset>(PrefabDataLocation);
            string json = ta.text;
            if(string.IsNullOrEmpty(json))
                return null;
            PrefabData data = Newtonsoft.Json.JsonConvert.DeserializeObject<PrefabData>(json);
            return data;
        }

        public void SetPrefabData(string json) 
        {
            if (string.IsNullOrEmpty(json))
                return;
            PrefabData = Newtonsoft.Json.JsonConvert.DeserializeObject<PrefabData>(json);
            PrefabDataLocation = null;
        }

        public void OnClickHook(System.Func<PrefabData, Texture2D, string, string, string, bool> setPanel) 
        {
            onClick = setPanel;
        }

        private void Update()
        {
            if (RenderTextureMethod != null) 
            {
                RenderTextureMethod.Invoke();
            }
        }
    }
}