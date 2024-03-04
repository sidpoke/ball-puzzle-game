using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Menu Page Identifier and Page Object
/// </summary>
[System.Serializable]
public struct MenuPageItem {
    public string MenuPageName;
    public GameObject MenuPageObject;
}

/// <summary>
/// Switches between "Pages" of the Menu, simply disabling and enabling game objects in a scene
/// </summary>
public class MainMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<MenuPageItem> menuObjects = new List<MenuPageItem>();

    [Header("Menu Setup")]
    [SerializeField] private string startMenu;
    [SerializeField] private string currentMenu;

    private void Start()
    {
        SetMenu(startMenu); //set start menu
    }

    public void SetMenu(string name) //Called by buttons
    {
        GameService.Instance.audioManager.PlaySound("UIButton");
        menuObjects.ForEach(menu => { menu.MenuPageObject.SetActive(menu.MenuPageName == name); });
    }

    public void LoadScene(string name) //Called by buttons
    {
        GameService.Instance.audioManager.PlaySound("UIButton");
        GameService.Instance.scenes.LoadScene(name);
    }
}
