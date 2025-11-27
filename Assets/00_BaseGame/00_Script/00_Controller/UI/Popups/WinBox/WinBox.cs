using System.Collections.Generic;
using DG.Tweening;
using Spine; 
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinBox : BoxSingleton<WinBox>
{
    public static WinBox Setup()
    {
        return Path(PathPrefabs.WIN_BOX);
    }

    public RectTransform progressBar;
    public Image imgFill;
    public Button btnNext;
    public SkeletonGraphic giftSkeleton;
    public Button giftOpen; 
    
    [Header("Rewards")]
    public Button btnClaimX2;
    public Button btnClaim;
    public TextMeshProUGUI txtTitle;
    public RectTransform description;
    public RectTransform rewards;
    public List<Image> lsRewards;
    public List<TextMeshProUGUI> txtRewards;
    
    [Header("Reward Amounts")]
    public int boosterAmount = 1;
    public int coinAmount = 100;
    public int heartMinutes = 30;
    private GiftType _selectedBooster;

    protected override void Init()
    {
        btnNext.onClick.AddListener(OnClickNext);
        
        btnClaim.onClick.AddListener(delegate
        {
            OnClickClaim();
            Close();
        });
        btnClaimX2.onClick.AddListener(delegate
        {
            OnClickClaim(true);
            Close();
        });

        // Đăng ký sự kiện click mở hộp
        giftOpen.onClick.AddListener(OnClickOpenGift);
        // Ban đầu ẩn nút mở hộp đi, chưa cho bấm
        giftOpen.enabled = false;
        
        PlayDropSequence();
        HandleProgress();
    }

    protected override void InitState()
    {
    }

    // --- LOGIC ANIMATION SPINE ---

    private void PlayDropSequence()
    {
        giftSkeleton.AnimationState.SetAnimation(0, "0-drop", false);
        giftSkeleton.AnimationState.AddAnimation(0, "1-loop", true, 0);
    }

    private void PlayOpenSequence()
    {
        TrackEntry track = giftSkeleton.AnimationState.SetAnimation(0, "2-open", false);
        giftSkeleton.AnimationState.AddAnimation(0, "3-open-loop", true, 0);
        
        // Đợi animation mở xong mới hiện UI nhận quà
        track.Complete += (entry) =>
        {
            ShowRewardUI();
        };
    }

    // -----------------------------

    private void HandleProgress()
    {
        btnNext.enabled = false;
        float oldProgressValue = UseProfile.LevelWinBoxProgress;
        float oldFill = oldProgressValue / 5f;
        UseProfile.LevelWinBoxProgress++; 
        float newProgressValue = UseProfile.LevelWinBoxProgress;
        float newFill = newProgressValue / 5f;
        imgFill.fillAmount = oldFill; 
        imgFill.DOFillAmount(newFill, 1.5f).SetEase(Ease.OutBack).OnComplete(delegate
        {
            if (newProgressValue >= 5)
            {
                UseProfile.LevelWinBoxProgress = 0;
                txtTitle.text = "Unlock Rewards";
                HandlePhaseUnlockReward();
            }
            else
            {
                btnNext.enabled = true; 
            }
        });
    }

    private void HandlePhaseUnlockReward()
    {
        progressBar.gameObject.SetActive(false);
        btnNext.gameObject.SetActive(false);
        description.gameObject.SetActive(true);
        giftSkeleton.rectTransform.DOLocalMoveY(-131f, 0.2f);
        giftOpen.enabled = true;
    }
    private void OnClickOpenGift()
    {
        giftOpen.enabled = false;
        HandleGiftRewardsData();
        PlayOpenSequence();
    }
    private void ShowRewardUI()
    {
        rewards.gameObject.SetActive(true);
        description.gameObject.SetActive(false);
        btnClaim.gameObject.SetActive(true);
        btnClaimX2.gameObject.SetActive(true);
    }

    private void HandleGiftRewardsData()
    {
        var dataGift = GameController.Instance.dataContains.giftData;
        
        var boosterTypes = new[]
        {
            GiftType.BoosterHint,
            GiftType.BoosterMagicWand,
            GiftType.BoosterFrozenTime
        };
        GiftType selectedBooster = boosterTypes[Random.Range(0, boosterTypes.Length)];
        _selectedBooster = selectedBooster;
        
        bool valid = true;
        valid &= dataGift.GetGift(selectedBooster, out Gift boosterGift);
        valid &= dataGift.GetGift(GiftType.Coin, out Gift coinGift);
        valid &= dataGift.GetGift(GiftType.HeartUnlimit, out Gift heartGift);
        
        if (!valid)
        {
            Debug.LogError("Thiếu cấu hình gift trong GiftDataBase!");
            return;
        }
        
        lsRewards[0].sprite = boosterGift.giftSprite;
        lsRewards[1].sprite = coinGift.giftSprite;
        lsRewards[2].sprite = heartGift.giftSprite;
        
        foreach(var reward in lsRewards)
            UIImageUtils.FitToTargetHeight(reward, 120);
        
        txtRewards[0].text = $"x{boosterAmount}";
        txtRewards[1].text = $"x{coinAmount}";
        txtRewards[2].text = $"{heartMinutes}m";
    }

    private void OnClickNext()
    {
        Close();
        UseProfile.Star += GamePlayController.Instance.gameScene.GetStarAmount();
        SelectGameModeBox.Setup().Show();
    }

    private void OnClickClaim(bool isX2 = false)
    {
        var giftData = GameController.Instance.dataContains.giftData;
        int multiplier = isX2 ? 2 : 1;
        
        giftData.Claim(_selectedBooster, boosterAmount * multiplier);
        giftData.Claim(GiftType.Coin, coinAmount * multiplier);
        giftData.Claim(GiftType.Heart, heartMinutes * multiplier);
        
        SelectGameModeBox.Setup().Show();
    }
}