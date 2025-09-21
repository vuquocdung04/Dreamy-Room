using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyLoginBox : BoxSingleton<DailyLoginBox>
{
    public static DailyLoginBox Setup()
    {
        return Path(PathPrefabs.DAILY_LOGIN_BOX); 
    }

    public Sprite claimedSprite;
    public Sprite originalSprite;
    public Button btnClose;
    public Button btnClaim;
    
    public List<DailyLoginItem> lsItems = new();
    protected override void Init()
    {
        btnClose.onClick.AddListener(Close);
    }

    protected override void InitState()
    {
        
    }

    private void OnClick()
    {
        
    }
}