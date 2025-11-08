using UnityEngine;

/// <summary>
/// Dung SO de debug trong luc code,
/// Luc BUILD GAME => CHUYEN THANH C#
/// </summary>



[CreateAssetMenu(fileName = "Player", menuName = "DATA/DATA PLAYER", order = 0)]
public class DataPlayer : ScriptableObject
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

    public void Save()
    {
        JsonSaveSystem.Save(this, "PlayerData");
    }

    public void Load()
    {
        JsonSaveSystem.LoadInto(this,"PlayerData");
    }
}