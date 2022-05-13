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
        private GameObject checkoutButton;
        [SerializeField]
        private UnityEngine.UI.Text Result;
        [SerializeField]
        private UnityEngine.UI.Text TotalDisplay;
        [SerializeField]
        private string successMessage = "Purchase successful!";
        [SerializeField]
        private string failureMessage = "Error: Insufficient funds";
        [SerializeField]
        private string emptyCartMessage = "Error: Cart is empty";
        [SerializeField]
        private float itemSpawnDelayInSeconds = 0.1f;
        [SerializeField]
        private float failureMessageDurationInSeconds = 2f;
        private List<GameObject> CartItemList = new List<GameObject>();
        private float itemSize = 100;
        private float itemMargin = 10;
        private bool checkoutWasPressed;
        private float total = 0.00f;

        public UnityEvent OnItemSpawn;

        private ComputerManager computerManager;

        PrefabData selected;

        private Queue<System.Func<GameObject>> SetPositions;

        private void RenderCart()
        {
            DestroyCart();
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
            if(CartItemList.Count == 0)
            {
                panel.gameObject.SetActive(false);
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
            computerManager = FindObjectOfType<ComputerManager>();
            RenderCart();
            ShowCheckoutButton();
            checkoutWasPressed = false;
            UpdateTotal();
            ItemPanel.onCartModified += UpdateTotal;
        }

        private void OnDisable()
        {
            DestroyCart();
            List<PrefabData> itemsToRemove = new List<PrefabData>();
            foreach(var item in computerManager.shoppingCart)
            {
                if(item.Value == 0)
                {
                    itemsToRemove.Add(item.Key);
                }
            }
            foreach(var item in itemsToRemove)
            {
                computerManager.shoppingCart.Remove(item);
            }
            if(checkoutWasPressed)
            {
                computerManager.shoppingCart.Clear();
                UpdateTotal();
            }
            ItemPanel.onCartModified -= UpdateTotal;
        }

        public bool SetSelected(PrefabData data, Texture2D icon, ShopItemData itemData)
        {
            panel.gameObject.SetActive(true);
            panel.UpdatePanel(data, icon, itemData);
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

            TotalDisplay.text = "Total:" + $"${total.ToString("0.00")}";
        }

        private void UpdateTotal()
        {
            total = 0.00f;
            bool cartHasItems = false;
            if(computerManager.shoppingCart.Count > 0)
            {
                foreach(var item in computerManager.shoppingCart)
                    if (item.Value > 0)
                    {
                        cartHasItems = true;
                        break;
                    }
            }
            if(cartHasItems)
            {
                foreach(var item in CartItemList)
                {
                    int quantity = computerManager.shoppingCart[item.GetComponent<ShopItem>().PrefabData];
                    total += (item.GetComponent<ShopItem>().ShopItemData.Price * quantity);
                }
            }
        }

        public void Checkout()
        {
            UpdateTotal();
            bool cartHasItems = false;
            if (computerManager.shoppingCart.Count > 0)
            {
                foreach (var item in computerManager.shoppingCart)
                {
                    if (item.Value > 0)
                    {
                        cartHasItems = true;
                        break;
                    }
                }
            }
            if (cartHasItems)
            {
                bool purchaseSuccessful;
                if (total <= FindObjectOfType<Player.PlayerController>().GetComponent<Player.Money>().Value)
                {
                    purchaseSuccessful = true;
                    FindObjectOfType<Player.PlayerController>().GetComponent<Player.Money>().Value -= total;
                    StartCoroutine(SpawnItems(computerManager.shoppingCart));
                    checkoutWasPressed = true;
                }
                else
                {
                    purchaseSuccessful = false;
                }
                StartCoroutine(ShowResultMessage(purchaseSuccessful));
            }
            else
            {
                StartCoroutine(ShowCartEmptyMessage());
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
            ShowCheckoutButton();
        }

        private void ShowCheckoutButton()
        {
            Result.enabled = false;
            checkoutButton.SetActive(true);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(ViewPort); //restore checkout button's normal color
        }

        private IEnumerator ShowResultMessage(bool _purchaseSuccessful)
        {
            checkoutButton.SetActive(false);
            Result.enabled = true;
            if(_purchaseSuccessful)
            {
                Result.text = successMessage;
                yield return null;
            }
            else
            {
                Result.text = failureMessage;
                yield return new WaitForSeconds(failureMessageDurationInSeconds);
                ShowCheckoutButton();
            }
        }

        private IEnumerator ShowCartEmptyMessage()
        {
            checkoutButton.SetActive(false);
            Result.enabled = true;
            Result.text = emptyCartMessage;
            yield return new WaitForSeconds(failureMessageDurationInSeconds);
            ShowCheckoutButton();
        }
    }
}
