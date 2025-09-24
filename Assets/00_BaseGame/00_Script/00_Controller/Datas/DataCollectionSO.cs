using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DataCollection", menuName = "DATA/DataCollection", order = 6)]
public class DataCollectionSO : ScriptableObject
{
    [SerializeField] private CollectionType currentCollectionType;
    public CollectionType GetType() => currentCollectionType;
    public void SetCollectionType(CollectionType collectionType) => currentCollectionType = collectionType;
    
    public List<CollectionConflict> lstCollectionConflict;

    public CollectionConflict GetCollectionByType(CollectionType type)
    {
        foreach(var collection in lstCollectionConflict)
            if (collection.type == type)  return collection;
        return null;
    }
}

[System.Serializable]
public class CollectionConflict
{
    [Title("Type")]
    [HorizontalGroup("GeneralInfo", Width = 0.3f, LabelWidth = 40)]
    [HideLabel]
    public CollectionType type;
    [Title("Total")]
    [HorizontalGroup("GeneralInfo", Width = 0.2f, LabelWidth = 80)]
    [HideLabel]
    public int totalAmount;
    [Title("Title")]
    [HorizontalGroup("GeneralInfo", LabelWidth = 40)]
    [HideLabel]
    public string txtTitle;
    
    public string txtDescription;
    public int amountReward;
    [PreviewField(50)]
    public Sprite sprReward;
    
    public List<int> lsIdCards;
    
    public int GetCount() => lsIdCards.Count;
}


public enum CollectionType
{
    None = 0,
    DreamScape = 1,
    CloudNine = 2,
    PoeticMuse = 3,
    ShowTime = 4,
    CozyCorner = 5,
    EastSide = 6,
    ChillGlow = 7,
    SunnyDays = 8,
    WishfulHeart = 9,
    TastyDream = 10,
    Fantasy = 11,
    Evermore = 12,
    SeaBreeze = 13,
    WonderScape = 14,
    Dream = 15,
    Future = 16,
    GreenVibes = 17,
    Grand =18,
    FairytaleEcho = 19,
    ChildHood = 20,
    Fabulous = 21,
    Soulmate = 22,
    True = 23,
    StarrySkies = 24,
    GameOn = 25
}