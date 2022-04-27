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

        private PrefabData activeItem;

        private int activeItemQuantity;

        private Dictionary<PrefabData, int> shoppingCart;

        public void UpdatePanel(PrefabData data, Texture2D newIcon, string title, string description) 
        {
            gameObject.SetActive(true);
            activeItem = data;
            Title.text = title;
            Description.text = description; 
            Icon.sprite = Sprite.Create(newIcon, new Rect(0,0,128,128), Vector2.zero);
            GetCartReference();
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
            GetCartReference();
            SetQuantityDisplay(activeItemQuantity);
        }

        private void GetCartReference()
        {
            shoppingCart = FindObjectOfType<ComputerManager>().shoppingCart;
        }

        public void IncrementQuantity()
        {
            activeItemQuantity++;
            SetQuantityDisplay(activeItemQuantity);
            UpdateShoppingCart();
        }

        public void DecrementQuantity()
        {
            activeItemQuantity--;
            int minQuantity = 0;
            if (activeItemQuantity < minQuantity)
            {
                activeItemQuantity = minQuantity;
            }
            SetQuantityDisplay(activeItemQuantity);
            UpdateShoppingCart();
        }

        public void AddItemToCart()
        {
            GetCartReference();
            if (!shoppingCart.ContainsKey(activeItem))
            {
                if (activeItemQuantity > 0)
                {
                    shoppingCart.Add(activeItem, activeItemQuantity);
                    UpdateShoppingCart();
                }
            }
            else
            {
                shoppingCart.Remove(activeItem);
                shoppingCart.Add(activeItem, activeItemQuantity);
                UpdateShoppingCart();
            }
        }

        private void UpdateShoppingCart()
        {
            FindObjectOfType<ComputerManager>().shoppingCart.Clear();
            foreach (KeyValuePair<PrefabData, int> item in shoppingCart)
            {
                FindObjectOfType<ComputerManager>().shoppingCart.Add(item.Key, item.Value);
            }
        }
    }
}