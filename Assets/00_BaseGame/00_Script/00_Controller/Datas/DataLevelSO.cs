using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataLevel", menuName = "DATA/DataLevelSO", order = 4)]
public class DataLevelSO : ScriptableObject
{
    [SerializeField] private List<LevelConflict> lstLevelConflicts;

    private Dictionary<int, LevelConflict> levelDictionary;

    public void Init()
    {
        levelDictionary = new Dictionary<int, LevelConflict>();
        foreach (var level in lstLevelConflicts)
        {
            if (!levelDictionary.ContainsKey(level.idLevel)) levelDictionary.Add(level.idLevel, level);
            else Debug.LogError("Duplicate");
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
    public int idLevel;
    public Sprite thumbnailIcon;
    public GameObject prefab;
}