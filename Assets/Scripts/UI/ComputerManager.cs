using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PersistentData.Saving;

namespace UI
{
    public class ComputerManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject Desktop;
        [SerializeField]
        private GameObject Shop;
        [SerializeField]
        private GameObject Cart;

        [SerializeField]
        private GameObject DesktopDefault;
        [SerializeField]
        private GameObject ShopDefault;
        [SerializeField]
        private GameObject CartDefault;

        private delegate void MenuAction();
        private MenuAction OpenLastMenu;

        public List<TextAsset> allItems = new List<TextAsset>();
        public List<GameObject> itemList = new List<GameObject>();
        public Dictionary<PrefabData, int> shoppingCart;

        private void AllOff()
        {
            Desktop.SetActive(false);
            Shop.SetActive(false);
            Cart.SetActive(false);
        }

        public void ShowDesktop()
        {
            AllOff();
            Desktop.SetActive(true);
            EventSystem.current.SetSelectedGameObject(DesktopDefault);
            OpenLastMenu = ShowDesktop;
        }

        public void OpenShop()
        {
            AllOff();
            Shop.SetActive(true);
            EventSystem.current.SetSelectedGameObject(ShopDefault);
            OpenLastMenu = OpenShop;
        }

        public void OpenCart()
        {
            AllOff();
            Cart.SetActive(true);
            EventSystem.current.SetSelectedGameObject(CartDefault);
            OpenLastMenu = OpenCart;
        }

        public void TurnOn()
        {
            OpenLastMenu?.Invoke();
        }

        public void TurnOff()
        {
            shoppingCart.Clear();
            AllOff();
        }

        void Start()
        {
            OpenLastMenu = ShowDesktop;
            shoppingCart = new Dictionary<PrefabData, int>();
            AllOff();
        }

        private void OnEnable()
        {
            Computer.ItemPanel.onItemRemoved += OpenShop;
            Computer.ItemPanel.onItemRemoved += OpenCart;
        }

        private void OnDisable()
        {
            Computer.ItemPanel.onItemRemoved -= OpenShop;
            Computer.ItemPanel.onItemRemoved -= OpenCart;
        }
    }
}