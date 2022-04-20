using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

        private void AllOff()
        {
            Desktop.SetActive(false);
            Shop.SetActive(false);
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
            AllOff();
        }

        void Start()
        {
            OpenLastMenu = ShowDesktop;
            AllOff();
        }
    }
}