using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIScreen : MonoBehaviour
{
    public string Name => gameObject.name;
    public UIScreen ParentScreen;
    public List<UIScreen> ChildrenScreens;

    private void Start()
    {
        if (ParentScreen == null)
        {
            var parent = this.transform.parent.GetComponent<UIScreen>();
            if (parent != null)
            {
                ParentScreen = parent;
            }
        }
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++) 
        {
            var child = transform.GetChild(i);
            UIScreen screen = child.GetComponent<UIScreen>();
            if (screen != null)
                continue;
            ChildrenScreens.Add(screen);
        }
        
    }

    public void CloseWholeMenu() 
    {
        CloseMenu();
        if (ParentScreen != null)
        {
            CloseWholeMenu();
        }
    }
    public void CloseMenu() 
    {
        this.gameObject.SetActive(false);
    }
    public void OpenMenu() 
    {
        this.gameObject.SetActive(true);
    }
    public void OpenParent() 
    {
        if (ParentScreen == null)
        {
            Console.LogError($"There is no Parent UI for \"{Name}\".");
        }
        else 
        {
            ParentScreen.OpenMenu();
            this.CloseMenu();
        }
    }
    public void OpenRoot() 
    {
        if (ParentScreen != null) 
        {
            ParentScreen.OpenMenu();
            this.CloseMenu();
            ParentScreen.OpenRoot();
        }
    }
    public void OpenScreen(string ScreenName) 
    {
        var obj = ChildrenScreens.FirstOrDefault(x => x.Name.ToLower().Equals(ScreenName.ToLower()));
        if (obj == null)
        {
            Console.LogError($"\"{ScreenName}\" could not be found. UIScreen, \"{Name}\" chould not be switched.");
        }
        else
        {
            obj.OpenMenu();
            this.CloseMenu();
        }
    }
    public void OpenSiblingScreen(string ScreenName) 
    {
        if (ParentScreen == null)
        {
            Console.LogError($"There is no Parent UI for \"{Name}\". As such, we can't locate siblings");
        }
        else 
        {
            ParentScreen.OpenScreen(ScreenName);
            CloseMenu();
        }
    }
}
