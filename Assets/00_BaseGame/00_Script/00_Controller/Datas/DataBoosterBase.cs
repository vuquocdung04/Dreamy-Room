using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DataBooster", menuName = "DATA/Data Booster", order = 0)]
public class DataBoosterBase : ScriptableObject
{
    public GiftType boosterTypeSeleced;
    
    public List<BoosterConflict> lsBoosters;

    public BoosterConflict GetBoosterConflict(GiftType bType)
    {
        foreach (var booster in lsBoosters)
            if (booster.type == bType)
                return booster;
        return null;
    }
    
}

[System.Serializable]
public class BoosterConflict
{
    [HorizontalGroup("TopRow", Width = 0.4f), HideLabel]
    public GiftType type;

    [HorizontalGroup("TopRow", Width = 0.3f)]
    [SerializeField] private int levelUnlock;
    [HorizontalGroup("TopRow"), LabelWidth(40)]
    [SerializeField] private int price = 800;
    
    [HorizontalGroup("BottomRow")]
    [SerializeField] string localizeKey;
    
    [HorizontalGroup("BottomRow", Width = 60)]
    [PreviewField(57, ObjectFieldAlignment.Center), HideLabel]
    [SerializeField] Sprite icon;

    public int GetLevelUnlock() => levelUnlock;
    public int GetPrice() => price;
    public string GetLocalizeKey() => localizeKey;
    public Sprite GetIcon() => icon;
    
}