using System.Collections.Generic;
using EventDispatcher;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionDetailBox : BoxSingleton<CollectionDetailBox>
{
    public static CollectionDetailBox Setup()
    {
        return Path(PathPrefabs.COLLECTION_DETAIL_BOX);
    }

    public CollectionType type;
    [Header("Title")] public Image imgTitle;
    public TextMeshProUGUI txtTitle;
    [Header("Progress")] public Image fill;
    public TextMeshProUGUI txtProgress;
    public Image imgReward;
    public TextMeshProUGUI txtRewardProgress;
    [Header("Description")] public TextMeshProUGUI txtDescription;
    [Header("Sprite Card")] public Sprite sprCardOn;
    public Button btnClose;

    public List<CollectionDetailItem> lsItems;
    private List<CollectionDetailItem> lsItemClones;

    protected override void Init()
    {
        var dataCollection = GameController.Instance.dataContains.dataCollection;
        type = dataCollection.GetType();
        btnClose.onClick.AddListener(Close);
    }

    protected override void InitState()
    {
        UpdateStateItems();
    }
    

    private void UpdateStateItems()
    {
        var dataCollection = GameController.Instance.dataContains.dataCollection;
        var dataLevel = GameController.Instance.dataContains.dataLevel;


        var dataCollectionDetail = dataCollection.GetCollectionByType(type);

        lsItemClones.Clear();
        foreach (var item in this.lsItems) item.gameObject.SetActive(false);

        for (int i = 0; i < dataCollectionDetail.GetCount(); i++)
        {
            lsItems[i].gameObject.SetActive(true);
            lsItems[i].SetId(dataCollectionDetail.lsIdCards[i]);
            lsItemClones.Add(lsItems[i]);
        }

        
        for (int i = 0; i < lsItemClones.Count; i++)
        {
            var spriteThumb = dataLevel.GetLevelSpriteById(lsItemClones[i].GetId());
            var isLevelUnlock = lsItemClones[i].GetId() < UseProfile.MaxUnlockedLevel;
            lsItemClones[i].Init(spriteThumb, sprCardOn, isLevelUnlock);
        }
    }
}