using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
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

    protected override void Init()
    {
        btnOpenGift.enabled = false;
        btnNext.onClick.AddListener(OnClickNext);
        btnOpenGift.onClick.AddListener(OnClickGift);
        HandleProgress();
    }

    protected override void InitState()
    {
    }

    private void OnClickNext()
    {
    }

    private void OnClickGift()
    {
        AnimationOpen();
    }

    private void HandleProgress()
    {
        btnNext.enabled = false;
        UseProfile.LevelWinBoxProgress++;
        var curFill = UseProfile.LevelWinBoxProgress / 5f;
        imgFill.DOFillAmount(curFill, 1f).SetEase(Ease.OutBack).OnComplete(delegate
        {
            if (UseProfile.LevelWinBoxProgress >= 5)
            {
                UseProfile.LevelWinBoxProgress = 0;
                progressBar.gameObject.SetActive(false);
                btnNext.gameObject.SetActive(false);
                btnOpenGift.enabled = true;
                txtTitle.text = "Unlock Rewards";
            }
            else
            {
                btnNext.enabled = true;
            }
        });
    }

    [Button("Test")]
    private void AnimationOpen(TweenCallback callback = null)
    {
        var seq = DOTween.Sequence();
        
        seq.Append(gift.DOScale(new Vector2(1.4f, 0.7f), 0.33f).SetEase(Ease.OutSine));
        
        Vector3 originalTopPos = giftLid.localPosition;
    
        seq.Append(DOTween.Sequence()
            .Join(gift.DOScale(Vector2.one, 0.2f).SetEase(Ease.InSine))
            .Join(giftLid.DOLocalMoveY(originalTopPos.y + 200f, 0.25f).SetEase(Ease.OutQuad))
            .Join(giftLid.DOLocalRotate(new Vector3(0, 0, -30f), 0.3f).SetEase(Ease.OutQuad))
        );

        if (callback != null)
            seq.OnComplete(callback);

        seq.Play();
    }
}