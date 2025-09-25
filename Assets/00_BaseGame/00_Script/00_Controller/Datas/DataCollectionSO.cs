using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DataCollection", menuName = "DATA/DataCollection", order = 6)]
public class DataCollectionSO : ScriptableObject
{
    [SerializeField] private CollectionType currentCollectionType;
    public CollectionType GetCollectionType() => currentCollectionType;
    public void SetCollectionType(CollectionType collectionType) => currentCollectionType = collectionType;

    public List<CollectionConflict> lstCollectionConflict;

    public int GetListCollectionCount() => lstCollectionConflict.Count;

    public CollectionConflict GetCollectionByType(CollectionType type)
    {
        foreach (var collection in lstCollectionConflict)
            if (collection.type == type)
                return collection;
        return null;
    }
}

[System.Serializable]
public class CollectionConflict
{
    [HorizontalGroup("Split", 0.5f, LabelWidth = 80)]
    [VerticalGroup("Split/Left")]
    [BoxGroup("Split/Left/General Info"), Title("Type"), HideLabel]
    public CollectionType type;

    [BoxGroup("Split/Left/General Info"), Title("Total Amount"), HideLabel]
    public int totalAmount;

    [BoxGroup("Split/Left/General Info"), Title("Title"), HideLabel]
    public string txtTitle;

    [BoxGroup("Split/Left/General Info"), Title("Amount Reward"), HideLabel]
    public int amountReward;

    [BoxGroup("Split/Left/General Info")] [TextArea]
    public string txtDescription;

    [BoxGroup("Split/Left/General Info")] [PreviewField(50, ObjectFieldAlignment.Center), HideLabel]
    public Sprite sprReward;

    [HorizontalGroup("Split")] [BoxGroup("Split/Card IDs")]
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
    Grand = 18,
    FairytaleEcho = 19,
    ChildHood = 20,
    Fabulous = 21,
    Soulmate = 22,
    True = 23,
    StarrySkies = 24,
    GameOn = 25
}