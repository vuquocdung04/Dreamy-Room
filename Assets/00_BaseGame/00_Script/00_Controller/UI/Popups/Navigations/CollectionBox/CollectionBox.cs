using UnityEngine;

public class CollectionBox : BoxSingleton<CollectionBox>
{
    public static CollectionBox Setup()
    {
        return Path(PathPrefabs.COLLECTION_BOX);
    }
    
    protected override void Init()
    {
    }

    protected override void InitState()
    {
    }
}