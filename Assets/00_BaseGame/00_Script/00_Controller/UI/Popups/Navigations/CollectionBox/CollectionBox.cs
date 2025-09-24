using System.Collections.Generic;
using Sirenix.OdinInspector;

public class CollectionBox : BoxSingleton<CollectionBox>
{
    public static CollectionBox Setup()
    {
        return Path(PathPrefabs.COLLECTION_BOX);
    }

    public List<CollectionItem> lsItems;
    protected override void Init()
    {
        UpdateItemState();

        OnClick(lsItems, (item) => HanleSelection(delegate
        {
            GameController.Instance.dataContains.dataCollection.SetCollectionType(item.GetCollectionType());
            CollectionDetailBox.Setup().Show();
        }));
        
    }

    protected override void InitState()
    {
        
    }

    private void UpdateItemState()
    {
        var dataCollection = GameController.Instance.dataContains.dataCollection;

        for (int i = 0; i < dataCollection.GetListCollectionCount(); i++)
        {
            lsItems[i].Init();
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