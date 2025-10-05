using UnityEngine;

public class RankBox : BoxSingleton<RankBox>
{
    public Canvas canvas;
    public static RankBox Setup()
    {
        return Path(PathPrefabs.RANK_BOX);
    }
    protected override void Init()
    {
        canvas.worldCamera = Camera.main;
    }

    protected override void InitState()
    {
        
    }
}