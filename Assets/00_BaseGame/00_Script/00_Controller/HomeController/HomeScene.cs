
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
    [Header("Other UI"), Space(5)]
    public Button btnEditProfile;
    public Button btnRemoveAds;
    public Button btnSetting;
    public Button btnDailyLogin;
    public Button btnDailyReward;
    
    public void Init()
    {
        
    }

    public Transform zoneSelectBird;
    public Transform zoneClickPlay;
}
