using System.Collections.Generic;
using Sirenix.OdinInspector;
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
    public TextMeshProUGUI txtReward;
    [Header("Description")] public TextMeshProUGUI txtDescription;
    [Header("Sprite Card")] public Sprite sprCardOn;
    public Button btnClose;
    public List<CollectionDetailItem> lsItems;
    private readonly List<CollectionDetailItem> lsItemClones = new();

    private int currentLevelCompleted;
    protected override void Init()
    {
        var dataCollection = GameController.Instance.dataContains.dataCollection;
        type = dataCollection.GetType();
        btnClose.onClick.AddListener(Close);

        OnClick(lsItemClones, (item) => HandleSelection(delegate
        {
            //NOTE: PLAY LEVEL
            
            Debug.Log("PLAY LEVEL: "  + item.GetId());
        }));
    }

    protected override void InitState()
    {
        UpdateStateItems();
        UpdateStateBox();
    }

    private void UpdateStateBox()
    {
        var dataCollection = GameController.Instance.dataContains.dataCollection;
        var collectionConflict = dataCollection.GetCollectionByType(type);
        
        lsItemClones.Clear();
        txtTitle.text = collectionConflict.txtTitle;
        txtDescription.text = collectionConflict.txtDescription;
        imgReward.sprite = collectionConflict.sprReward;
        txtReward.text = collectionConflict.amountReward.ToString();
        
        for (int i = 0; i < collectionConflict.GetCount(); i++)
        {
            if(collectionConflict.lsIdCards[i] > UseProfile.MaxUnlockedLevel) continue;
            currentLevelCompleted++;
        }
        txtProgress.text = currentLevelCompleted.ToString() + "/" + collectionConflict.totalAmount;
        fill.fillAmount = (float) currentLevelCompleted/collectionConflict.totalAmount;
    }

    private void UpdateStateItems()
    {
        var dataCollection = GameController.Instance.dataContains.dataCollection;
        var dataLevel = GameController.Instance.dataContains.dataLevel;
        var collectionConflict = dataCollection.GetCollectionByType(type);
        foreach (var item in this.lsItems) item.gameObject.SetActive(false);

        for (int i = 0; i < collectionConflict.GetCount(); i++)
        {
            lsItems[i].gameObject.SetActive(true);
            lsItems[i].SetId(collectionConflict.lsIdCards[i]);
            lsItemClones.Add(lsItems[i]);
        }

        
        for (int i = 0; i < lsItemClones.Count; i++)
        {
            var spriteThumb = dataLevel.GetLevelSpriteById(lsItemClones[i].GetId());
            var isLevelUnlock = lsItemClones[i].GetId() <= UseProfile.MaxUnlockedLevel;
            lsItemClones[i].Init(spriteThumb, sprCardOn, isLevelUnlock);
        }
    }

    private void HandleSelection(System.Action callback = null)
    {
        callback?.Invoke();
    }
    private void OnClick(List<CollectionDetailItem> collectionDetail, System.Action<CollectionDetailItem> callback = null)
    {
        foreach (var t in collectionDetail)
        {
            t.AddClickListener(callback);
        }
    }

    [Button("Setup Item")]
    void SetupItem()
    {
        for (int i = 0; i < lsItems.Count; i++)
        {
            lsItems[i].SetupOdin();
        }
    }
}