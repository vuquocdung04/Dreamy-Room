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
    public Canvas canvas;
    [Header("Title")] public Image imgTitle;
    [Header("Progress")] public Image fill;
    public TextMeshProUGUI txtProgress;
    public Image imgReward;
    public TextMeshProUGUI txtReward;
    [Header("Sprite Card")] public Sprite sprCardOn;
    public Button btnClose;
    [Header("Localization")]
    public LocalizedText lcTitle;
    public LocalizedText lcDescription;
    public List<CollectionDetailItem> lsItems;
    
    private readonly List<CollectionDetailItem> lsItemClones = new();

    private int currentLevelCompleted;
    private DataCollectionBase dataCollection;
    protected override void Init()
    {
        canvas.worldCamera = Camera.main;
        dataCollection = GameController.Instance.dataContains.dataCollection;
        btnClose.onClick.AddListener(Close);

        OnClick(lsItems, (item) => HandleSelection(delegate
        {
            Debug.Log("PLAY LEVEL: "  + item.GetId());
            if(item.GetId() >= UseProfile.MaxUnlockedLevel) return;
            GameController.Instance.curGameModeName = GameMode.RELAX;
            GameController.Instance.ChangeScene2(SceneName.GAME_PLAY);
        }));
    }

    protected override void InitState()
    {
        type = dataCollection.GetCollectionType();
        
        UpdateStateItems();
        UpdateStateBox();
    }

    private void InitLocalization(CollectionConflict collectionConflict)
    {
        lcTitle.Init(collectionConflict.lcKeyTitle);
        lcDescription.Init(collectionConflict.lcKeyDesc);
    }
    
    private void UpdateStateBox()
    {
        
        var collectionConflict = dataCollection.GetCollectionByType(type);
        currentLevelCompleted = 0;
        lsItemClones.Clear();
        InitLocalization(collectionConflict);
        imgReward.sprite = collectionConflict.sprReward;
        txtReward.text = collectionConflict.amountReward.ToString();
        for (int i = 0; i < collectionConflict.GetCount(); i++)
        {
            if(collectionConflict.lsIdCards[i] > UseProfile.MaxUnlockedLevel) continue;
            currentLevelCompleted++;
        }
        txtProgress.text = currentLevelCompleted + "/" + collectionConflict.totalAmount;
        fill.fillAmount = (float) currentLevelCompleted/collectionConflict.totalAmount;
    }

    private void UpdateStateItems()
    {
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