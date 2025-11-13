using UnityEngine;

public class RankBox : BoxSingleton<RankBox>
{
    public Canvas canvas;
    public static RankBox Setup()
    {
        return Path(PathPrefabs.RANK_BOX);
    }

    public LocalizedText lcDesc;
    protected override void Init()
    {
        canvas.worldCamera = Camera.main;
        InitLocalization();
    }

    protected override void InitState()
    {
        RefreshLocalization(GameController.Instance.dataContains.DataPlayer, InitLocalization);
    }

    private void InitLocalization()
    {
        lcDesc.Init();
    }
}