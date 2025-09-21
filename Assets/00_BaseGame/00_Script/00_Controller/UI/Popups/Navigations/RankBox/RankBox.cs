using UnityEngine;

public class RankBox : BoxSingleton<RankBox>
{
    public static RankBox Setup()
    {
        return Path(PathPrefabs.RANK_BOX);
    }
    protected override void Init()
    {
        
    }

    protected override void InitState()
    {
        
    }
}