
public class HintBooster : BoosterBase
{
    protected override void OnBoosterUsed()
    {
        GamePlayController.Instance.levelController.currentLevel.UseHintBooster();
    }

    protected override void UpdateUseProfileAmount(int amount)
    {
        UseProfile.Booster_Hint = amount;
    }
}