using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class DEVTOOL : MonoBehaviour
{
    // --- TAB TIỀN TỆ (COIN, STAR & HEART) ---
    [TabGroup("DevToolTabs", "Currency")] [Title("Amount Settings", titleAlignment: TitleAlignments.Centered)]
    public int amountIncrease = 10000;

    [TabGroup("DevToolTabs", "Currency")]
    [Title("Coin & Star Management", titleAlignment: TitleAlignments.Centered)]
    [Button("Increase Coin", ButtonSizes.Large)]
    private void IncreaseCoin()
    {
        UseProfile.Coin += amountIncrease;
    }

    [TabGroup("DevToolTabs", "Currency")]
    [Button("Increase Star", ButtonSizes.Large)]
    private void IncreaseStar()
    {
        UseProfile.Star += amountIncrease;
    }

    [TabGroup("DevToolTabs", "Currency")]
    [HorizontalGroup("DevToolTabs/Currency/Resets")]
    [Button("Reset Coin", ButtonSizes.Medium)]
    private void ResetCoin()
    {
        UseProfile.Coin = 0;
    }

    [HorizontalGroup("DevToolTabs/Currency/Resets")]
    [Button("Reset Star", ButtonSizes.Medium)]
    private void ResetStar()
    {
        UseProfile.Star = 0;
    }

    [TabGroup("DevToolTabs", "Currency")]
    [Title("Heart Management", titleAlignment: TitleAlignments.Centered)]
    [ReadOnly]
    public bool isUnlimitHeart;

    [TabGroup("DevToolTabs", "Currency")]
    [Button("Toggle Heart Unlimit", ButtonSizes.Large)]
    private void ToggleUnlimitedHeart()
    {
        if (!isUnlimitHeart)
            isUnlimitHeart = true;
        else
            isUnlimitHeart = false;
        UseProfile.IsUnlimitedHeart = isUnlimitHeart;
    }


    // --- TAB THỜI GIAN ---
    [TabGroup("DevToolTabs", "Time")]
    [Button("Reset Day (Yesterday)", ButtonSizes.Large)]
    private void ResetDay()
    {
        UseProfile.TimeLastLoginDate = DateTime.Now.AddDays(-1);
    }

    [TabGroup("DevToolTabs", "Time")]
    [Button("Next Day", ButtonSizes.Large)]
    private void NextDay()
    {
        UseProfile.TimeLastLoginDate = DateTime.Now.AddDays(-1);
    }

    [TabGroup("DevToolTabs", "Time")]
    [Button("Next 2 Days", ButtonSizes.Large)]
    private void Next2Day()
    {
        UseProfile.TimeLastLoginDate = DateTime.Now.AddDays(-2);
    }


    // --- TAB LEVEL ---
    [TabGroup("DevToolTabs", "Level")] public int maxLevelUnlock = 100;

    [TabGroup("DevToolTabs", "Level")]
    [Button("Max Unlock Level", ButtonSizes.Large)]
    private void MaxUnlockLevel()
    {
        UseProfile.MaxUnlockedLevel = maxLevelUnlock;
    }

    [TabGroup("DevToolTabs", "Level")]
    [Button("Next Level", ButtonSizes.Large)]
    private void NextLevel()
    {
        if (UseProfile.CurrentLevel == UseProfile.MaxUnlockedLevel)
        {
            UseProfile.MaxUnlockedLevel++;
        }

        UseProfile.CurrentLevel++;
    }


    // --- TAB KHÁC ---
    [TabGroup("DevToolTabs", "Other")]
    [Button("Remove Ads", ButtonSizes.Large)]
    private void RemoveAds()
    {
        if (GameController.Instance != null && GameController.Instance.useProfile != null)
        {
            GameController.Instance.useProfile.IsRemoveAds = true;
        }
        else
        {
            Debug.LogError("GameController.Instance hoặc useProfile chưa được khởi tạo!");
        }
    }

    [TabGroup("DevToolTabs", "Other")]
    [Button("Next Scene Game", ButtonSizes.Large)]
    private void NextSceneGame()
    {
        GameController.Instance.effectChangeScene.FadeToScene(SceneName.GAME_PLAY);
    }
}