using UnityEngine;
using UnityEngine.UI;

public class PigBankBox :BoxSingleton<PigBankBox>
{
    public static PigBankBox Setup()
    {
        return Path(PathPrefabs.PIG_BANK_BOX);
    }

    public Button btnClose;
    public Button btnPurchase;
    public Image fill;

    private int totalProgress = 2400;
    protected override void Init()
    {
        UpdateFillState();
        btnClose.onClick.AddListener(Close);
        btnPurchase.onClick.AddListener(delegate
        {
            //NOTE: purchase logic
            Debug.Log("Purchase Pig Bank");
        });
    }

    protected override void InitState()
    {
        
    }

    private void UpdateFillState()
    {
        var totalCompletedLevel = UseProfile.MaxUnlockedLevel;
        var progress = totalCompletedLevel * 200;
        fill.fillAmount = (float)progress / totalProgress;
    }
    
}