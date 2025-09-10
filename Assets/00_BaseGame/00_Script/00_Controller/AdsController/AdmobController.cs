using UnityEngine;
using GoogleMobileAds.Api;
using EventDispatcher;

public class AdmobController : MonoBehaviour
{
     private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;
    private AppOpenAd appOpenAd;

    // Ad Unit IDs của bạn
    //private const string BannerId = "ca-app-pub-4739017290334481/4383883316"; // Bieu ngu
    //private const string InterstitialId = "ca-app-pub-4739017290334481/6239906374"; // Trung gian
    //private const string RewardedId = "ca-app-pub-4739017290334481/2604716024"; // Nhan thuong
    //private const string AppOpenId = "ca-app-pub-4739017290334481/8079601655";

    //Test
    private const string BannerId = "ca-app-pub-3940256099942544/6300978111";
    private const string InterstitialId = "ca-app-pub-3940256099942544/1033173712";
    private const string RewardedId = "ca-app-pub-3940256099942544/5224354917";
    private const string AppOpenId = "ca-app-pub-3940256099942544/3419835294";
    public void Init()
    {
        // Khởi tạo SDK
        MobileAds.Initialize(_ => { 
            Debug.Log("AdMob SDK initialized");
        });
        RequestInterstitial();
        RequestRewardedAd();
        RequestAppOpenAd();
    }

    #region BANNER ADS
    public void RequestBanner()
    {
        this.bannerView = new BannerView(BannerId, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest();
        this.bannerView.LoadAd(request);
        
        Debug.Log("Banner requested");
    }

    public void ShowBanner()
    {
        if(GameController.Instance.useProfile.IsRemoveAds) return;
        
        if (bannerView != null)
            bannerView.Show();
    }

    public void HideBanner()
    {
        if (bannerView != null)
            bannerView.Hide();
    }

    public void DestroyBanner()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }
    }
    #endregion

    #region INTERSTITIAL ADS
    public void RequestInterstitial()
    {
        InterstitialAd.Load(InterstitialId, new AdRequest(), (ad, error) =>
        {
            if (error != null)
            {
                Debug.LogError("Interstitial failed to load: " + error.GetMessage());
                return;
            }

            interstitialAd = ad;
            Debug.Log("Interstitial loaded");

            interstitialAd.OnAdFullScreenContentClosed += () => {
                Debug.Log("Interstiti al closed");
                RequestInterstitial();
            };

            interstitialAd.OnAdFullScreenContentFailed += adError => {
                Debug.LogError("Interstitial failed to show: " + adError.GetMessage());
                RequestInterstitial();
            };
        });
    }

    public void ShowInterstitial()
    {
        if(GameController.Instance.useProfile.IsRemoveAds) return;
        
        if (interstitialAd != null)
        {
            interstitialAd.Show();
            Debug.Log("Showing Interstitial");
        }
        else
        {
            Debug.Log("Interstitial not ready");
        }
    }
    #endregion

    #region REWARDED ADS
    public void RequestRewardedAd()
    {
        RewardedAd.Load(RewardedId, new AdRequest(), (ad, error) =>
        {
            if (error != null)
            {
                Debug.LogError("Rewarded ad failed to load: " + error.GetMessage());
                return;
            }

            rewardedAd = ad;
            Debug.Log("Rewarded ad loaded");

            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded ad closed");
                RequestRewardedAd();
            };

            rewardedAd.OnAdFullScreenContentFailed += adError => {
                Debug.LogError("Rewarded ad failed to show: " + adError.GetMessage());
                RequestRewardedAd();
            };
        });
    }

    private void HandleUserEarnedReward(Reward args)
    {
        Debug.Log("User earned reward: " + args.Amount + " " + args.Type);
        
        // Gửi event thông báo nhận thưởng thành công
        this.PostEvent(EventID.REWARDED_ADS_COMPLETED, args);
    }

    public void ShowRewardedAd()
    {
        if (GameController.Instance.useProfile.IsRemoveAds)
        {
            var fakeReward = new Reward()
            {
                Type = "LevelUnlock",
                Amount = 1,
            };
            HandleUserEarnedReward(fakeReward);
            return;
        }
        
        // Kiểm tra xem quảng cáo đã được tải thành công chưa
        if (rewardedAd != null)
        {
            // Hiển thị quảng cáo và truyền vào callback để xử lý khi người dùng nhận thưởng
            // reward => { HandleUserEarnedReward(reward); } tương đương với việc viết:
            // void MyCallback(Reward reward) { HandleUserEarnedReward(reward); }
            // rewaredAd.Show(MyCallBack)
            rewardedAd.Show(reward =>
            {
                // Gọi hàm xử lý phần thưởng khi người dùng xem xong quảng cáo
                HandleUserEarnedReward(reward);
            });
        
            Debug.Log("Showing Rewarded Ad");
        }
        else
        {
            // Quảng cáo chưa sẵn sàng để hiển thị
            Debug.Log("Rewarded ad not ready");
        }
    }
    #endregion
    
    

    #region APP OPEN ADS
    public void RequestAppOpenAd()
    {
        AppOpenAd.Load(AppOpenId, new AdRequest(), (ad, error) =>
        {
            if (error != null)
            {
                Debug.LogError("AppOpenAd failed to load: " + error.GetMessage());
                return;
            }

            appOpenAd = ad;
            Debug.Log("AppOpen ad loaded");

            appOpenAd.OnAdFullScreenContentClosed += () => {
                Debug.Log("AppOpen ad closed");
                RequestAppOpenAd();
            };

            appOpenAd.OnAdFullScreenContentFailed += adError => {
                Debug.LogError("AppOpen ad failed to show: " + adError.GetMessage());
                RequestAppOpenAd();
            };
        });
    }

    public void ShowAppOpenAd()
    {
        if (appOpenAd != null)
        {
            appOpenAd.Show();
            Debug.Log("Showing AppOpen Ad");
        }
        else
        {
            Debug.Log("AppOpen ad not ready");
        }
    }
    #endregion
}
