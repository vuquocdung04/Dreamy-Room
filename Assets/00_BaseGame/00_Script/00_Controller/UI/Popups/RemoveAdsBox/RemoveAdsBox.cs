
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveAdsBox : BoxSingleton<RemoveAdsBox>
{
    public static RemoveAdsBox Setup()
    {
        return Path(PathPrefabs.REMOVE_ADS_BOX);
    }

    public Button btnClose;
    public List<LocalizedText> lsLcs;
    
    protected override void Init()
    {
        btnClose.onClick.AddListener(delegate
        {
            Close();
            if(GamePlayController.Instance)
                GamePlayController.Instance.ResumeGame();
            else
                Debug.LogError("GamePlayController is null");
        });
        InitLocalization();
    }

    protected override void InitState()
    {
        RefreshLocalization(GameController.Instance.dataContains.DataPlayer, InitLocalization);
    }

    private void InitLocalization()
    {
        foreach(var t in lsLcs) t.Init();
    }
}