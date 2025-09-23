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
        });
    }

    protected override void InitState()
    {
    }
}