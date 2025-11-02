using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DataLevel", menuName = "DATA/DataLevelSO", order = 4)]
public class DataLevelBase : ScriptableObject
{
    public GameObject levelTutorial;

    [ListDrawerSettings(
        NumberOfItemsPerPage = 14,
        ShowIndexLabels = true,
        ShowPaging = true,
        DraggableItems = false,
        HideAddButton = false
    )]
    [SerializeField]
    private List<LevelConflict> lstLevelConflicts;

    private Dictionary<int, LevelConflict> levelDictionary;

    public void Init()
    {
        levelDictionary = new Dictionary<int, LevelConflict>();
        foreach (var level in lstLevelConflicts)
        {
            if (!levelDictionary.TryAdd(level.idLevel, level))
                Debug.LogError("Duplicate");
        }
    }

    public GameObject GetLevelPrefabById(int id)
    {
        if (levelDictionary.TryGetValue(id, out LevelConflict levelConflict)) return levelConflict.prefab;
        return null;
    }

    public Sprite GetLevelSpriteById(int id)
    {
        if (levelDictionary.TryGetValue(id, out LevelConflict levelConflict)) return levelConflict.thumbnailIcon;
        return null;
    }

    public Sprite GetBgSpriteById(int id)
    {
        if (levelDictionary.TryGetValue(id, out LevelConflict levelConflict)) return levelConflict.backGround;
        return null;
    }
}

[System.Serializable]
public class LevelConflict
{
    [HorizontalGroup("Main", Width = 0.3f), HideLabel]
    public int idLevel;

    [HorizontalGroup("Main", Width = 0.4f), HideLabel]
    public GameObject prefab;

    [HorizontalGroup("Main"), HideLabel] [PreviewField(50, ObjectFieldAlignment.Right)]
    public Sprite thumbnailIcon;

    [HorizontalGroup("Main"), HideLabel] [PreviewField(50, ObjectFieldAlignment.Right)]
    public Sprite backGround;
}