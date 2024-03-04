using UnityEngine;

/// <summary>
/// Spawns a button for each level in the adventure mode menu layout box
/// </summary>
public class AdventureMenu : MonoBehaviour
{
    [SerializeField] private GameObject layoutParent;
    [SerializeField] private GameObject buttonPrefab;

    public void Start()
    {
        SpawnLevelButtons();
    }

    /// <summary>
    /// Spawns buttons for adventure mode menu
    /// </summary>
    private void SpawnLevelButtons()
    {
        for (int i = 0; i < GameService.Instance.adventure.Levels.Count; i++)
        {
            GameObject btn = Instantiate(buttonPrefab, layoutParent.transform);
            btn.GetComponent<Button_SelectAdventure>().SetLevel(i, GameService.Instance.adventure.Levels[i].LevelName);
        }
    }
}
