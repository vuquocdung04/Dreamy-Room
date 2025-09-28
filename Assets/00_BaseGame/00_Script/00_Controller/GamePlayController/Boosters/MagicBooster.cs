using UnityEngine;

public class MagicBooster : BoosterBase
{
    protected override void OnBoosterUsed()
    {
        
    }

    protected override void UpdateUseProfileAmount(int amount)
    {
        UseProfile.Booster_MagicWand =  amount;
    }
}