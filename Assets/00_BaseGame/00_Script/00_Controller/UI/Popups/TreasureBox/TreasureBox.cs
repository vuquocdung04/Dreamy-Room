using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreasureBox :BoxSingleton<TreasureBox>
{
    public static TreasureBox Setup()
    {
        return Path(PathPrefabs.TREASURE_BOX);
    }

    public Button btnContinue;
    public Button btnClose;
    public Image fillProgress;
    public TextMeshProUGUI txtFill;
    protected override void Init()
    {
        UpdateFillProgress();
        btnClose.onClick.AddListener(delegate
        {
            Close();
        });
        btnContinue.onClick.AddListener(delegate
        {
            OnBtnContinue();
        });
    }

    protected override void InitState()
    {
    }

    private void OnBtnContinue()
    {
        //Chuyen Scene
    }
    
    private void UpdateFillProgress()
    {
        var star = UseProfile.Star;
        txtFill.text = star.ToString();
        var progress = (float)star / 400;
        fillProgress.fillAmount = progress;
    }
}