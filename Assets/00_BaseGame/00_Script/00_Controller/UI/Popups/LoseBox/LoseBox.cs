using UnityEngine;

public class LoseBox : BoxSingleton<LoseBox>
{
    public static LoseBox Setup()
    {
        return Path(PathPrefabs.LOSE_BOX);
    }
    protected override void Init()
    {
    }

    protected override void InitState()
    {
    }
}