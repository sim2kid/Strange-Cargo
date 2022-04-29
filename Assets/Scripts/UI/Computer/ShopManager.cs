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
        private float itemSize = 100;
        private float itemMargin = 10;

        PrefabData selected;

        private ComputerManager computerManager;
        private Queue<System.Func<GameObject>> SetPositions;

        private void RenderShop()
        {
            DestroyShop();
            RectTransform rT = ViewPort.GetComponent<RectTransform>();
            float maxSize = itemMargin + (Mathf.Ceil(computerManager.itemList.Count / 2f) * (itemSize + itemMargin));
            rT.sizeDelta = new Vector2(0, maxSize);
            for (int i = 0; i < computerManager.itemList.Count; i++)
            {
                GameObject obj = computerManager.itemList[i];
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

        private void DestroyShop()
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
            DestroyShop();
        }

        public bool SetSelected(PrefabData data, Texture2D icon, string title, string description, string price) 
        {
            panel.UpdatePanel(data, icon, title, description, price);
            return true;
        }

        void Start()
        {
            computerManager = FindObjectOfType<ComputerManager>();
            if (!string.IsNullOrEmpty(ShopItemResourceFolder)) 
            {
                var all = Resources.LoadAll<TextAsset>(ShopItemResourceFolder);
                foreach (var item in computerManager.itemList) 
                {
                    if (item.scene.IsValid()) 
                    {
                        Destroy(item);
                    }
                }
                computerManager.itemList.Clear();
                computerManager.allItems.Clear();
                foreach (var item in all) 
                {
                    computerManager.allItems.Add(item);
                    var obj = Instantiate(ShopItem);
                    var shop = obj.GetComponent<ShopItem>();
                    shop.SetPrefabData(item.text);
                    computerManager.itemList.Add(obj);
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