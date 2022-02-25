using PersistentData.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Computer
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject ViewPort;
        [SerializeField]
        private string ShopItemResourceFolder;
        [SerializeField]
        private GameObject ShopItem;
        [SerializeField]
        private ItemPanel panel;
        private List<GameObject> ItemList = new List<GameObject>();
        private float itemSize = 100;
        private float itemMargin = 10;

        PrefabData selected;

        private Queue<System.Func<GameObject>> SetPositions;

        private void RenderShop()
        {
            DestoryShop();
            RectTransform rT = ViewPort.GetComponent<RectTransform>();
            float maxSize = itemMargin + (Mathf.Ceil(ItemList.Count / 2f) * (itemSize + itemMargin));
            rT.sizeDelta = new Vector2(0, maxSize);
            for (int i = 0; i < ItemList.Count; i++)
            {
                GameObject obj = ItemList[i];
                obj.transform.position = rT.position;
                obj.transform.rotation = rT.rotation;
                obj.transform.SetParent(rT);
                obj.transform.localScale = Vector3.one;
                obj.SetActive(true);

                obj.GetComponent<ShopItem>().OnClickHook(SetSelected);

                RectTransform objRT = obj.GetComponent<RectTransform>();
                int down = (int)Mathf.Ceil(i / 2);
                float x = itemMargin + ((i % 2) * (itemMargin + itemSize)) + (itemSize / 2);
                float y = itemMargin + ((itemSize + itemMargin) * down) + (itemSize / 2);
                SetPositions.Enqueue(() =>
                {
                    objRT.localPosition = new Vector3(x, -y, objRT.localPosition.z);
                    return objRT.gameObject;
                });
            }
        }

        private void DestoryShop()
        {
            foreach (Transform child in ViewPort.transform)
                child.gameObject.SetActive(true);
            RectTransform rT = ViewPort.GetComponent<RectTransform>();
            rT.sizeDelta = new Vector2(0, 0);
            selected = null;
        }

        private void OnEnable()
        {
            SetPositions = new Queue<System.Func<GameObject>>();
            RenderShop();
        }

        private void OnDisable()
        {
            DestoryShop();
        }

        public bool SetSelected(PrefabData data, Texture2D icon, string title, string description) 
        {
            panel.UpdatePanel(data, icon, title, description);
            return true;
        }

        void Start()
        {
            if (!string.IsNullOrEmpty(ShopItemResourceFolder)) 
            {
                var all = Resources.LoadAll<TextAsset>(ShopItemResourceFolder);
                foreach (var item in ItemList) 
                {
                    if (item.scene.IsValid()) 
                    {
                        Destroy(item);
                    }
                }
                ItemList.Clear();
                foreach (var item in all) 
                {
                    var obj = Instantiate(ShopItem);
                    var shop = obj.GetComponent<ShopItem>();
                    shop.SetPrefabData(item.text);
                    ItemList.Add(obj);
                    obj.SetActive(false);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (SetPositions.Count > 0)
            {
                foreach (var runMe in SetPositions)
                {
                    runMe.Invoke();
                }
            }
            SetPositions.Clear();
        }
    }
}