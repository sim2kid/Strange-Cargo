using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PersistentData.Saving;
using UnityEngine.Events;

namespace UI.Computer
{
    public class CartManager : MonoBehaviour
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
        private TextAsset[] allItems;

        public UnityEvent OnItemSpawn;

        private Dictionary<PrefabData, int> shoppingCart;

        PrefabData selected;

        private Queue<System.Func<GameObject>> SetPositions;

        private void RenderCart()
        {
            DestroyCart();
            shoppingCart = FindObjectOfType<ComputerManager>().shoppingCart;
            foreach (var item in allItems)
            {
                var obj = Instantiate(ShopItem);
                var shop = obj.GetComponent<ShopItem>();
                shop.SetPrefabData(item.text);
                foreach(KeyValuePair<PrefabData, int> _item in shoppingCart)
                {
                    if(_item.Key.Equals(shop.PrefabData))
                    {
                        ItemList.Add(obj);
                    }
                }
                obj.SetActive(false);
            }

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

        private void DestroyCart()
        {
            foreach (Transform child in ViewPort.transform)
                child.gameObject.SetActive(true);
            RectTransform rT = ViewPort.GetComponent<RectTransform>();
            rT.sizeDelta = new Vector2(0, 0);
            selected = null;
            ItemList.Clear();
        }

        private void OnEnable()
        {
            ItemPanel.OnItemRemove += DestroyCart;
            ItemPanel.OnItemRemove += RenderCart;
            SetPositions = new Queue<System.Func<GameObject>>();
            RenderCart();
        }

        private void OnDisable()
        {
            ItemPanel.OnItemRemove -= DestroyCart;
            ItemPanel.OnItemRemove -= RenderCart;
            DestroyCart();
        }

        public bool SetSelected(PrefabData data, Texture2D icon, string title, string description)
        {
            panel.UpdatePanel(data, icon, title, description);
            return true;
        }

        void Awake()
        {
            if (!string.IsNullOrEmpty(ShopItemResourceFolder))
            {
                allItems = Resources.LoadAll<TextAsset>(ShopItemResourceFolder);
                foreach (var item in ItemList)
                {
                    if (item.scene.IsValid())
                    {
                        Destroy(item);
                    }
                }
                ItemList.Clear();
                
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

        public void Checkout()
        {
            foreach (KeyValuePair<PrefabData, int> item in shoppingCart)
            {
                for (int i = 1; i <= item.Value; i++)
                {
                    FindObjectOfType<ItemSpawner>().Spawn(item.Key);
                    OnItemSpawn.Invoke();
                }
            }
        }
    }
}
