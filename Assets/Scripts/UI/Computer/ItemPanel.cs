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
        private TMPro.TextMeshProUGUI Price;
        [SerializeField]
        private UnityEngine.UI.Image Icon;
        [SerializeField]
        private InputField quantityDisplay;

        private PrefabData activeItem;

        private int activeItemQuantity;

        private ComputerManager computerManager;

        public void UpdatePanel(PrefabData data, Texture2D newIcon, string title, string description) 
        {
            gameObject.SetActive(true);
            activeItem = data;
            Title.text = title;
            Description.text = description;
            Icon.sprite = Sprite.Create(newIcon, new Rect(0,0,128,128), Vector2.zero);
            if (FindObjectOfType<CartManager>() != null && computerManager.shoppingCart.ContainsKey(activeItem))
            {
                foreach (KeyValuePair<PrefabData, int> item in computerManager.shoppingCart)
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
            computerManager = FindObjectOfType<ComputerManager>();
            SetQuantityDisplay(activeItemQuantity);
        }

        public void IncrementQuantity()
        {
            activeItemQuantity++;
            SetQuantityDisplay(activeItemQuantity);
            UpdateActiveItemQuantityInCart();
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
            UpdateActiveItemQuantityInCart();
        }

        private void UpdateActiveItemQuantityInCart()
        {
            if (FindObjectOfType<CartManager>() != null)
            {
                computerManager.shoppingCart[activeItem] = activeItemQuantity;
            }
        }

        public void AddItemToCart()
        {
            if (!computerManager.shoppingCart.ContainsKey(activeItem))
            {
                if (activeItemQuantity > 0)
                {
                    computerManager.shoppingCart.Add(activeItem, activeItemQuantity);
                }
            }
            else
            {
                computerManager.shoppingCart[activeItem] += activeItemQuantity;
            }
        }
    }
}