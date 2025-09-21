using UnityEngine;

public class RemoveAdsBox : BoxSingleton<RemoveAdsBox>
{
    public static RemoveAdsBox Setup()
    {
        return Path(PathPrefabs.REMOVE_ADS_BOX);
    }
    protected override void Init()
    {
    }

    protected override void InitState()
    {
    }
}