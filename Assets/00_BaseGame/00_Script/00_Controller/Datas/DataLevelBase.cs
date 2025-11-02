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
        return levelDictionary.TryGetValue(id, out LevelConflict levelConflict) ? levelConflict.backGround : null;
    }

    public Sprite GetPatternById(int id)
    {
        return levelDictionary.TryGetValue(id, out LevelConflict levelConflict) ? levelConflict.pattern : null;
    }
}

[System.Serializable]
public class LevelConflict
{
    [HorizontalGroup("Row1")]
    [VerticalGroup("Row1/Col1"), Title("ID"), HideLabel]
    public int idLevel;

    [VerticalGroup("Row1/Col2"), Title("Level Prefab"), HideLabel]
    public GameObject prefab;

    [VerticalGroup("Row1/Col3"), Title("Thumbnail"), HideLabel]
    [PreviewField(70, ObjectFieldAlignment.Center)]
    public Sprite thumbnailIcon;

    [VerticalGroup("Row1/Col4"), Title("Background"), HideLabel]
    [PreviewField(70, ObjectFieldAlignment.Center)]
    public Sprite backGround;
    
    [VerticalGroup("Row1/Col5"), Title("Pattern"), HideLabel]
    [PreviewField(70, ObjectFieldAlignment.Center)]
    public Sprite pattern;
}