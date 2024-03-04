using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdventureMenu : MonoBehaviour
{
    [SerializeField] private GameObject layoutParent;
    [SerializeField] private GameObject buttonPrefab;

    public void Start()
    {
        SpawnLevelButtons();
    }

    /// <summary>
    /// Spawns a button for each level in the adventure box
    /// </summary>
    private void SpawnLevelButtons()
    {
        for (int i = 0; i < GameService.Instance.adventure.Levels.Count; i++)
        {
            GameObject btn = Instantiate(buttonPrefab, layoutParent.transform) as GameObject;
            btn.GetComponent<Button_SelectAdventure>().SetLevel(i, GameService.Instance.adventure.Levels[i].LevelName);
        }
    }
}
