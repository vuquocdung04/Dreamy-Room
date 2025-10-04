using EventDispatcher;
using UnityEngine;

public class FrozeBooster : BoosterBase
{
    
    public override void Init(int curBoosterAmount)
    {
        base.Init(curBoosterAmount);
        this.RegisterListener(EventID.ON_FROZE_BOOSTER_ENDED,EnableBtn);
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.ON_FROZE_BOOSTER_ENDED, EnableBtn);
    }
    
    private void EnableBtn(object obj = null)
    {
        btn.enabled = true;
    }
    
    protected override void OnBoosterUsed()
    {
        btn.enabled = false;
        GamePlayController.Instance.levelController.currentLevel.UseFrozeBooster();
    }

    protected override void UpdateUseProfileAmount(int amount)
    {
        UseProfile.Booster_FrozeTime = amount;
    }
}