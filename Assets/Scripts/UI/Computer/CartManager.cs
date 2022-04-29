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
        [SerializeField]
        private float itemSpawnDelayInSeconds = 0.1f;
        private List<GameObject> CartItemList = new List<GameObject>();
        private float itemSize = 100;
        private float itemMargin = 10;

        public UnityEvent OnItemSpawn;

        private ComputerManager computerManager;

        PrefabData selected;

        private Queue<System.Func<GameObject>> SetPositions;

        private void RenderCart()
        {
            DestroyCart();
            computerManager = FindObjectOfType<ComputerManager>();
            foreach (var item in computerManager.itemList)
            {
                var shop = item.GetComponent<ShopItem>();
                foreach(KeyValuePair<PrefabData, int> _item in computerManager.shoppingCart)
                {
                    if(_item.Key.Equals(shop.PrefabData))
                    {
                        CartItemList.Add(item);
                    }
                }
            }

            RectTransform rT = ViewPort.GetComponent<RectTransform>();
            float maxSize = itemMargin + (Mathf.Ceil(CartItemList.Count / 2f) * (itemSize + itemMargin));
            rT.sizeDelta = new Vector2(0, maxSize);
            for (int i = 0; i < CartItemList.Count; i++)
            {
                GameObject obj = CartItemList[i];
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
            CartItemList.Clear();
        }

        private void OnEnable()
        {
            SetPositions = new Queue<System.Func<GameObject>>();
            RenderCart();
        }

        private void OnDisable()
        {
            DestroyCart();
        }

        public bool SetSelected(PrefabData data, Texture2D icon, string title, string description, string price)
        {
            panel.UpdatePanel(data, icon, title, description, price);
            return true;
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
            float total = 0.00f;
            foreach(var item in CartItemList)
            {
                total += item.GetComponent<ShopItem>().ItemPrice;
            }
            if (total <= FindObjectOfType<Player.PlayerController>().GetComponent<Player.Money>().Value)
            {
                StartCoroutine(SpawnItems(computerManager.shoppingCart));
            }
        }

        private IEnumerator SpawnItems(Dictionary<PrefabData, int> _itemsToSpawn)
        {
            var itemSpawner = FindObjectOfType<ItemSpawner>();
            foreach(KeyValuePair<PrefabData, int> item in _itemsToSpawn)
            {
                if(item.Value > 0)
                {
                    for(int i = 1; i <= item.Value; i++)
                    {
                        yield return new WaitForSeconds(itemSpawnDelayInSeconds);
                        itemSpawner.Spawn(item.Key);
                        OnItemSpawn.Invoke();
                    }
                }
            }
        }
    }
}
