using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreasureBox : BoxSingleton<TreasureBox>
{
    public static TreasureBox Setup()
    {
        return Path(PathPrefabs.TREASURE_BOX);
    }

    public Button btnClaim;
    public TextMeshProUGUI txtBtnClaim;
    public Button btnClose;
    public Image fillProgress;
    public TextMeshProUGUI txtFill;
    public int totalStar = 400;

    protected override void Init()
    {
        btnClose.onClick.AddListener(delegate { Close(); });
        btnClaim.onClick.AddListener(delegate { OnBtnClaim(); });
        UpdateFillProgress();
        UpdateStateBtnClaim();
    }

    protected override void InitState()
    {
    }

    private void OnBtnClaim()
    {
        if (CanClaim())
        {
            GameController.Instance.dataContains.giftData.DeDuct(GiftType.Star, totalStar);
            UpdateFillProgress();
        }
        else
        {
            Close();
            UseProfile.CurrentLevel = UseProfile.MaxUnlockedLevel;
            SelectGameModeBox.Setup().Show();
        }
        // Logic qua o day =))
    }

    private bool CanClaim()
    {
        var star = UseProfile.Star;
        return star > totalStar;
    }

    private void UpdateStateBtnClaim()
    {
        txtBtnClaim.text = CanClaim() ? "Claim" : "Continue";
    }

    private void UpdateFillProgress()
    {
        var star = UseProfile.Star;
        txtFill.text = star.ToString();
        var progress = (float)star / totalStar;
        fillProgress.fillAmount = progress;
    }
}