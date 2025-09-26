using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DataBooster", menuName = "DATA/Data Booster", order = 0)]
public class DataBoosterBase : ScriptableObject
{
    public GiftType boosterTypeSeleced;
    
    public List<BoosterConflict> lsBoosters;

    public Sprite GetSpriteByType(GiftType bType)
    {
        foreach (var booster in lsBoosters)
            if (booster.type == bType)
                return booster.icon;
        return null;
    }

    public int GetPriceByType(GiftType bType)
    {
        foreach (var booster in lsBoosters)
            if (booster.type == bType)
                return booster.price;
        return 0;
    }

    public string GetDescriptionByType(GiftType bType)
    {
        foreach (var booster in lsBoosters)
            if (booster.type == bType)
                return booster.description;
        return null;
    }
    
}

[System.Serializable]
public class BoosterConflict
{
    [HorizontalGroup("TopRow", Width = 0.6f), HideLabel]
    public GiftType type;

    [HorizontalGroup("TopRow"), LabelWidth(40)]
    public int price = 800;
    
    [HorizontalGroup("BottomRow")]
    [TextArea(3, 5), HideLabel] 
    public string description;
    
    [HorizontalGroup("BottomRow", Width = 60)]
    [PreviewField(57, ObjectFieldAlignment.Center), HideLabel]
    public Sprite icon;
}