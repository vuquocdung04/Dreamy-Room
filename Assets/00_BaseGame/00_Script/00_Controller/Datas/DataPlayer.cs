using UnityEngine;

/// <summary>
/// Dung SO de debug trong luc code,
/// Luc BUILD GAME => CHUYEN THANH C#
/// </summary>

public class DataPlayer 
{
    [Header("Booster Used")]
    public bool isUsedX2Star;
    public bool isUsedTimeBuffer;
    public bool isUsedBoxBuffer;
    [Header("DAILY LOGIN STREAK")]
    public int playerClaimedDay;
    public bool hasClaimedStreakToday;
    [Header("DAILY TIERED REWARDS")]
    public bool isFreeClaimedToday;
    public int adRewardsClaimedCount;
    [Header("Localization")]
    [SerializeField] private Language currentLanguage;
    [SerializeField] private Language previousLanguage;
    [SerializeField] private bool isLanguageChanged;
    public bool IsLanguageChanged => currentLanguage != previousLanguage;
    public Language CurrentLanguage
    {
        get => currentLanguage;
        set
        {
            previousLanguage = currentLanguage;
            currentLanguage = value;
            isLanguageChanged = currentLanguage != previousLanguage;
        }
    }
    public DataPlayer()
    {
        isUsedX2Star = false;
        isUsedTimeBuffer = false;
        isUsedBoxBuffer = false;
        playerClaimedDay = 0;
        hasClaimedStreakToday = false;
        isFreeClaimedToday = false;
        adRewardsClaimedCount = 0;
        currentLanguage = Language.En;
    }
}