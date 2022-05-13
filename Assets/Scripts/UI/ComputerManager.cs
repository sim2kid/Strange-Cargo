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
        private GameObject EmailQuest;
        [SerializeField]
        private GameObject Wiki;
        [SerializeField]
        private GameObject Taskbar;

        [SerializeField]
        private GameObject DesktopDefault;
        [SerializeField]
        private GameObject ShopDefault;
        [SerializeField]
        private GameObject CartDefault;
        [SerializeField]
        private GameObject EmailQuestDefault;
        [SerializeField]
        private GameObject WikiDefault;

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
            EmailQuest.SetActive(false);
            Wiki.SetActive(false);
        }

        public void ShowDesktop()
        {
            AllOff();
            Desktop.SetActive(true);
            Taskbar.SetActive(true);
            EventSystem.current.SetSelectedGameObject(DesktopDefault);
            OpenLastMenu = ShowDesktop;
        }

        public void OpenShop()
        {
            AllOff();
            Shop.SetActive(true);
            Taskbar.SetActive(false);
            EventSystem.current.SetSelectedGameObject(ShopDefault);
            OpenLastMenu = OpenShop;
        }

        public void OpenCart()
        {
            AllOff();
            Cart.SetActive(true);
            Taskbar.SetActive(false);
            EventSystem.current.SetSelectedGameObject(CartDefault);
            OpenLastMenu = OpenCart;
        }

        public void OpenEmailQuest()
        {
            AllOff();
            EmailQuest.SetActive(true);
            Taskbar.SetActive(true);
            EventSystem.current.SetSelectedGameObject(EmailQuestDefault);
            OpenLastMenu = OpenEmailQuest;
        }

        public void OpenWiki()
        {
            AllOff();
            Wiki.SetActive(true);
            Taskbar.SetActive(true);
            EventSystem.current.SetSelectedGameObject(WikiDefault);
            OpenLastMenu = OpenWiki;
        }

        public void TurnOn()
        {
            Taskbar.SetActive(true);
            OpenLastMenu?.Invoke();
        }

        public void TurnOff()
        {
            Taskbar.SetActive(false);
            shoppingCart.Clear();
            AllOff();
        }

        void Start()
        {
            OpenLastMenu = ShowDesktop;
            shoppingCart = new Dictionary<PrefabData, int>();
            AllOff();
        }
    }
}