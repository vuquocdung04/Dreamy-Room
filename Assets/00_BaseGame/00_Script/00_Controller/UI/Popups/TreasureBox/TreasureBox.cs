using UnityEngine;

public class TreasureBox :BoxSingleton<TreasureBox>
{
    public static TreasureBox Setup()
    {
        return Path(PathPrefabs.TREASURE_BOX);
    }
    protected override void Init()
    {
    }

    protected override void InitState()
    {
    }
}