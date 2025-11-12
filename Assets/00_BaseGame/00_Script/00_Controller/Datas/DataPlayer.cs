using UnityEngine;

/// <summary>
/// Dung SO de debug trong luc code,
/// Luc BUILD GAME => CHUYEN THANH C#
/// </summary>

public class DataPlayer 
{
    [Header("Booster Used")]
    public bool IsUsedX2Star;
    public bool IsUsedTimeBuffer;
    public bool IsUsedBoxBuffer;
    [Header("DAILY LOGIN STREAK")]
    public int PlayerClaimedDay;
    public bool HasClaimedStreakToday;
    [Header("DAILY TIERED REWARDS")]
    public bool IsFreeClaimedToday;
    public int ADRewardsClaimedCount;
    // Localization
    public Language CurrentLanguage;
    private Language previousLanguage;
    public bool IsLanguageChanged => CurrentLanguage != previousLanguage;
    public DataPlayer()
    {
        IsUsedX2Star = false;
        IsUsedTimeBuffer = false;
        IsUsedBoxBuffer = false;
        PlayerClaimedDay = 0;
        HasClaimedStreakToday = false;
        IsFreeClaimedToday = false;
        ADRewardsClaimedCount = 0;
        CurrentLanguage = Language.En;
        previousLanguage = Language.En;
    }
    
    public void SetCurrentLanguage(Language language)
    {
        previousLanguage = CurrentLanguage;
        CurrentLanguage = language;
    }
    
    public void ResetLanguageChangeFlag()
    {
        previousLanguage = CurrentLanguage;
    }
}