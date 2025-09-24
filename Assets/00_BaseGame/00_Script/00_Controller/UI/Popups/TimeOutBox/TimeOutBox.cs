using UnityEngine;
using UnityEngine.UI;

public class TimeOutBox : BoxSingleton<TimeOutBox>
{
    public static TimeOutBox Setup()
    {
        return Path(PathPrefabs.TIME_OUT_BOX);
    }

    public Button btnClose;
    public Button btnContinueByCoin;
    public Button btnContinueByAd;
    public Button btnPurchase;
    
    protected override void Init()
    {
        btnClose.onClick.AddListener(delegate
        {
            LoseBox.Setup().Show();
            Close();
        });
        btnContinueByCoin.onClick.AddListener(delegate
        {
            
        });
        btnContinueByAd.onClick.AddListener(delegate
        {
            
        });
        btnPurchase.onClick.AddListener(delegate
        {
            
        });
    }

    protected override void InitState()
    {
    }
    
    
}