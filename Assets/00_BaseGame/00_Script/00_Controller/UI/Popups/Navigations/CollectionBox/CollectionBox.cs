using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CollectionBox : BoxSingleton<CollectionBox>
{
    public Canvas canvas;
    public static CollectionBox Setup()
    {
        return Path(PathPrefabs.COLLECTION_BOX);
    }

    public LocalizedText title;
    public LocalizedText grandPrize;
    public List<CollectionItem> lsItems;
    protected override void Init()
    {
        canvas.worldCamera = Camera.main;
        UpdateItemState();
    
        OnClick(lsItems, (item) => HanleSelection(delegate
        {
            GameController.Instance.dataContains.dataCollection.SetCollectionType(item.GetCollectionType());
            CollectionDetailBox.Setup().Show();
        }));
        
    }

    protected override void InitState()
    {
        HandleLocalization();
    }

    private void HandleLocalization()
    {
        Debug.Log("Check");
        title.Init();
        grandPrize.Init();
    }

    private void UpdateItemState()
    {
        var dataCollection = GameController.Instance.dataContains.dataCollection;

        foreach(var item in lsItems) item.HandleInteractableBtn(false);
        
        for (int i = 0; i < dataCollection.GetListCollectionCount(); i++)
        {
            lsItems[i].Init();
            lsItems[i].HandleInteractableBtn(true);
        }
    }

    private void HanleSelection(System.Action callback = null)
    {
        callback?.Invoke();
    }

    private void OnClick(List<CollectionItem> lstItems, System.Action<CollectionItem> callback = null)
    {
        
        foreach (var t in lstItems)
            t.AddClickListener(callback);
    }

    [Button("Setup Item")]
    void SetupItem()
    {
        for (int i = 0; i < lsItems.Count; i++)
        {
            lsItems[i].SetupOdin(i+1);
        }
    }
}