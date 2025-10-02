using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DataLevel", menuName = "DATA/DataLevelSO", order = 4)]
public class DataLevelBase : ScriptableObject
{
    public GameObject levelTutorial;
    
    [SerializeField] private List<LevelConflict> lstLevelConflicts;

    private Dictionary<int, LevelConflict> levelDictionary;

    public int GetCountList() => lstLevelConflicts.Count;
    
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
    
}

[System.Serializable]
public class LevelConflict
{
    [HorizontalGroup("Split",Width = 0.3f)]
    public int idLevel;
    
    [HorizontalGroup("Split",Width = 0.5f)]
    public GameObject prefab;
    
    [HorizontalGroup("Split"),HideLabel]
    [PreviewField(50, ObjectFieldAlignment.Right)]
    public Sprite thumbnailIcon;
}