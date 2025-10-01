
using UnityEngine;
using UnityEngine.UI;

public class RemoveAdsBox : BoxSingleton<RemoveAdsBox>
{
    public static RemoveAdsBox Setup()
    {
        return Path(PathPrefabs.REMOVE_ADS_BOX);
    }

    public Button btnClose;
    
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
    }

    protected override void InitState()
    {
    }
}