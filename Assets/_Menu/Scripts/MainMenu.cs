using System.Linq;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct MenuPageItem {
    public string MenuPageName;
    public GameObject MenuPageObject;
}

public class MainMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<MenuPageItem> menuObjects = new List<MenuPageItem>();

    [Header("Menu Setup")]
    [SerializeField] private string startMenu;
    [SerializeField] private string currentMenu;

    private void Start()
    {
        SetMenu(startMenu);
    }

    //Called by buttons
    public void SetMenu(string name)
    {
        menuObjects.ForEach(menu => { menu.MenuPageObject.SetActive(menu.MenuPageName == name); });
    }

    public void LoadScene(string name)
    {
        GameService.Instance.scenes.LoadScene(name);
    }
}
