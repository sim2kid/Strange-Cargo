using PersistentData.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
        [SerializeField]
        private InputField quantityDisplay;

        public UnityEvent OnItemSpawn;

        private PrefabData activeItem;

        private int activeItemQuantity;

        private Dictionary<PrefabData, int> shoppingCart;

        public delegate void ItemRemoved();
        public static event ItemRemoved OnItemRemove;

        public void UpdatePanel(PrefabData data, Texture2D newIcon, string title, string description) 
        {
            gameObject.SetActive(true);
            activeItem = data;
            Title.text = title;
            Description.text = description; 
            Icon.sprite = Sprite.Create(newIcon, new Rect(0,0,128,128), Vector2.zero);
            if (shoppingCart.ContainsKey(activeItem))
            {
                foreach (KeyValuePair<PrefabData, int> item in shoppingCart)
                {
                    if (item.Key == activeItem)
                    {
                        activeItemQuantity = item.Value;
                    }
                }
            }
            else
            {
                activeItemQuantity = 0;
            }
            SetQuantityDisplay(activeItemQuantity);
        }

        private void SetQuantityDisplay(int _quantity)
        {
            quantityDisplay.text = _quantity.ToString();
        }

        void Start() 
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            shoppingCart = FindObjectOfType<CartManager>().shoppingCart;
        }

        public void Checkout()
        {
            foreach (KeyValuePair<PrefabData, int> item in shoppingCart)
            {
                FindObjectOfType<ItemSpawner>().Spawn(item.Key);
                for (int i = 0; i < item.Value; i++)
                {
                    OnItemSpawn.Invoke();
                }
            }
        }

        public void IncrementQuantity()
        {
            activeItemQuantity++;
            SetQuantityDisplay(activeItemQuantity);
        }

        public void DecrementQuantity()
        {
            activeItemQuantity--;
            int minQuantity = 0;
            if (GetComponentInParent<ShopManager>() != null)
            {
                minQuantity = 0;
            }
            else if(GetComponentInParent<CartManager>() != null)
            {
                minQuantity = 1;
            }
            if (activeItemQuantity < minQuantity)
            {
                activeItemQuantity = minQuantity;
            }
            SetQuantityDisplay(activeItemQuantity);
        }

        public void AddItemToCart()
        {
            if (activeItemQuantity > 0)
            {
                shoppingCart.Add(activeItem, activeItemQuantity);
                UpdateShoppingCart();
            }
        }

        public void RemoveItemFromCart()
        {
            if(shoppingCart.ContainsKey(activeItem))
            {
                shoppingCart.Remove(activeItem);
                UpdateShoppingCart();
            }
            OnItemRemove.Invoke();
            gameObject.SetActive(false);
        }

        private void UpdateShoppingCart()
        {
            FindObjectOfType<CartManager>().shoppingCart = shoppingCart;
        }
    }
}