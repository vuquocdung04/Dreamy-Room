using UnityEngine;

public class RateBox : BoxSingleton<RateBox>
{
    public static RateBox Setup()
    {
        return Path(PathPrefabs.RATE_BOX);
    }
    protected override void Init()
    {
    }

    protected override void InitState()
    {
    }
}