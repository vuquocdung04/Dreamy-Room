using UnityEngine;

public class CollectionDetailBox : BoxSingleton<CollectionDetailBox>
{
    public static CollectionDetailBox Setup()
    {
        return Path(PathPrefabs.COLLECTION_DETAIL_BOX);
    }
    protected override void Init()
    {
    }

    protected override void InitState()
    {
    }
}