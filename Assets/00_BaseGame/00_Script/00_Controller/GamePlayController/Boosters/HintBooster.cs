
public class HintBooster : BoosterBase
{
    protected override void OnBoosterUsed()
    {
        
    }

    protected override void UpdateUseProfileAmount(int amount)
    {
        UseProfile.Booster_Hint = amount;
    }
}