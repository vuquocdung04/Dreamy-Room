
using UnityEngine;
using UnityEngine.UI;

public class HomeScene : MonoBehaviour
{
    [Header("Navigation Bar")]
    public Button btnShop;
    public Button btnRank;
    public Button btnHome;
    public Button btnTeam;
    public Button btnCollection;
    [Header("Hotkey"),Space(5)]
    public Button btnShopShortcut;
    
    [Header("Other UI"), Space(5)]
    public Button btnEditProfile;
    public Button btnRemoveAds;
    public Button btnSetting;
    public Button btnDailyLogin;
    public Button btnDailyReward;
    public Button btnStarter;
    public Button btnPigBank;
    public Button btnTreasure;
    
    
    public void Init()
    {
        
    }
}
