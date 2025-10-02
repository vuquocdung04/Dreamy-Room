using UnityEngine;

public class FrozeBooster : BoosterBase
{
    protected override void OnBoosterUsed()
    {
        GamePlayController.Instance.levelController.currentLevel.UseFrozeBooster();
    }

    protected override void UpdateUseProfileAmount(int amount)
    {
        UseProfile.Booster_FrozeTime = amount;
    }
}