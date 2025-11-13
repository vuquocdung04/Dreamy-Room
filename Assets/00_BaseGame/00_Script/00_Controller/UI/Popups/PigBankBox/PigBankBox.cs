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
    public LocalizedText lcTitle;
    public LocalizedText lcDesc;

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
        
        InitLocalization();
    }

    protected override void InitState()
    {
        RefreshLocalization(GameController.Instance.dataContains.DataPlayer, InitLocalization);
    }

    private void InitLocalization()
    {
        lcDesc.Init();
        lcTitle.Init();
    }

    private void UpdateFillState()
    {
        var totalCompletedLevel = UseProfile.MaxUnlockedLevel;
        var progress = totalCompletedLevel * 200;
        fill.fillAmount = (float)progress / totalProgress;
    }
    
}