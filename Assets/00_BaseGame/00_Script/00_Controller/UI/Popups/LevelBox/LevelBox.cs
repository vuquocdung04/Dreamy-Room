using UnityEngine;

public class LevelBox : BoxSingleton<LevelBox>
{
    public static LevelBox Setup()
    {
        return Path(PathPrefabs.LEVEL_BOX);
    }
    protected override void Init()
    {
    }

    protected override void InitState()
    {
    }
}