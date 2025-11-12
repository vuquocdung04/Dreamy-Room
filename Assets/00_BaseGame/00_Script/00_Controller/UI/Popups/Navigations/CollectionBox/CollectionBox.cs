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
    public List<string> lsKeysTitle;
    private DataPlayer dataPlayer;
    protected override void Init()
    {
        canvas.worldCamera = Camera.main;
        dataPlayer = GameController.Instance.dataContains.DataPlayer;
        UpdateItemState();
    
        OnClick(lsItems, (item) => HanleSelection(delegate
        {
            GameController.Instance.dataContains.dataCollection.SetCollectionType(item.GetCollectionType());
            CollectionDetailBox.Setup().Show();
        }));
        InitLocalization();
    }

    protected override void InitState()
    {
        RefreshLocalization(dataPlayer,()=> InitLocalization());
    }

    private void InitLocalization()
    {
        title.Init();
        grandPrize.Init();
        foreach(var item in lsItems)
            item.GetLocalizedText().Init();
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


    [Button("Setup Item", ButtonSizes.Large)]
    private void SetupItem()
    {
        for (int i = 0; i < lsItems.Count; i++)
        {
            lsItems[i].SetupOdin(i+1);
        }
    }

    [Button("Setup Count lsKey", ButtonSizes.Large)]
    private void SetCountLsKey()
    {
        lsKeysTitle.Clear();
        while (lsKeysTitle.Count < lsItems.Count)
        {
            lsKeysTitle.Add(string.Empty);
        }
        while (lsKeysTitle.Count > lsItems.Count)
        {
            lsKeysTitle.RemoveAt(lsKeysTitle.Count - 1);
        }

    }
    [Button("Setup Key Item", ButtonSizes.Large)]
    private void SetupKeyItem()
    {
        for (int i = 0; i < lsItems.Count; i++)
        {
            lsItems[i].GetLocalizedText().SetKeyOnEditor(lsKeysTitle[i]);
        }
    }

}