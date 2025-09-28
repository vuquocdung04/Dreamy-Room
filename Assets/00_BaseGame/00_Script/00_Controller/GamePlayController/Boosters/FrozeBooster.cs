using UnityEngine;

public class FrozeBooster : BoosterBase
{
    protected override void OnBoosterUsed()
    {
    }

    protected override void UpdateUseProfileAmount(int amount)
    {
        UseProfile.Booster_FrozeTime = amount;
    }
}