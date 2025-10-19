using System.Collections.Generic;
using DG.Tweening;
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
    public RectTransform giftLid;
    public RectTransform gift;
    public Image imgFill;
    public Button btnNext;
    public Button btnOpenGift;
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
        btnOpenGift.enabled = false;
        btnNext.onClick.AddListener(OnClickNext);
        btnOpenGift.onClick.AddListener(OnClickGift);
        HandleProgress();
        btnClaim.onClick.AddListener(delegate
        {
            OnClickClaim();
        });
        btnClaimX2.onClick.AddListener(delegate
        {
            OnClickClaim(true);
        });
    }

    protected override void InitState()
    {
    }

    private void OnClickNext()
    {
        Close();
        UseProfile.Star += GamePlayController.Instance.gameScene.GetStarAmount();
        GameController.Instance.ChangeScene2(SceneName.GAME_PLAY);
    }

    private void OnClickClaim(bool isX2 = false)
    {
        var giftData = GameController.Instance.dataContains.giftData;
        int multiplier = isX2 ? 2 : 1;
        
        giftData.Claim(_selectedBooster, boosterAmount * multiplier);
        
        giftData.Claim(GiftType.Coin, coinAmount * multiplier);
        
        giftData.Claim(GiftType.Heart, heartMinutes * multiplier); 
    }
    
    private void OnClickGift()
    {
        btnOpenGift.enabled = false;
        description.gameObject.SetActive(false);
        AnimationOpen(delegate
        {
            btnClaim.gameObject.SetActive(true);
            btnClaimX2.gameObject.SetActive(true);
            HandleGiftRewards();
        });
        
    }

    private void HandleProgress()
    {
        btnNext.enabled = false;
        imgFill.fillAmount = UseProfile.LevelWinBoxProgress / 5f;
        UseProfile.LevelWinBoxProgress++;
        var curFill = UseProfile.LevelWinBoxProgress / 5f;
        imgFill.DOFillAmount(curFill, 1.5f).SetEase(Ease.OutBack).OnComplete(delegate
        {
            if (UseProfile.LevelWinBoxProgress >= 5)
            {
                UseProfile.LevelWinBoxProgress = 0;
                HandlePhaseUnlockReward();
            }
            else
            {
                btnNext.enabled = true;
            }
        });
    }
    private void AnimationOpen(TweenCallback callback = null)
    {
        var seq = DOTween.Sequence();
        
        seq.Append(gift.DOScale(new Vector2(1.4f, 0.7f), 0.33f).SetEase(Ease.OutSine));
        
        Vector3 originalTopPos = giftLid.localPosition;
    
        seq.Append(DOTween.Sequence()
            .Join(gift.DOScale(Vector2.one, 0.2f).SetEase(Ease.InSine))
            .Join(giftLid.DOLocalMoveY(originalTopPos.y + 400f, 0.25f).SetEase(Ease.OutQuad))
            .Join(giftLid.DOLocalRotate(new Vector3(0, 0, -30f), 0.3f).SetEase(Ease.OutQuad))
        );

        if (callback != null)
            seq.OnComplete(callback);

        seq.Play();
    }

    private void HandlePhaseUnlockReward()
    {
        progressBar.gameObject.SetActive(false);
        btnNext.gameObject.SetActive(false);
        btnOpenGift.enabled = true;
        txtTitle.text = "Unlock Rewards";
        description.gameObject.SetActive(true);
        gift.DOAnchorPosY(-248f,0.2f).SetEase(Ease.OutBack);
    }

    private void HandleGiftRewards()
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
        
        rewards.gameObject.SetActive(valid);

        if (!valid)
        {
            Debug.LogError("Thiếu cấu hình gift trong GiftDataBase! Vui lòng kiểm tra.");
            return;
        }
        
        lsRewards[0].sprite = boosterGift.giftSprite;
        lsRewards[1].sprite = coinGift.giftSprite;
        lsRewards[2].sprite = heartGift.giftSprite;
        
        foreach(var reward in lsRewards)
            UIImageUtils.FitToTargetHeight(reward,120);
        
        txtRewards[0].text = $"x{boosterAmount}";
        txtRewards[1].text = $"x{coinAmount}";
        txtRewards[2].text = $"{heartMinutes}m";
    }
}