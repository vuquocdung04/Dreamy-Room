using UnityEngine;

public class MagicBooster : BoosterBase
{
    protected override void OnBoosterUsed()
    {
        GamePlayController.Instance.levelController.currentLevel.UseMagicWandBooster();
    }

    protected override void UpdateUseProfileAmount(int amount)
    {
        UseProfile.Booster_MagicWand =  amount;
    }
}