using UnityEngine;

public class FrozeBooster : BoosterBase
{
    protected override void OnBoosterUsed()
    {
        GamePlayController.Instance.gameScene.ActivateFrozeBooster();
    }

    protected override void UpdateUseProfileAmount(int amount)
    {
        UseProfile.Booster_FrozeTime = amount;
    }
}