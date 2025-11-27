using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class DevToolWindow : OdinEditorWindow
{
    [MenuItem("Tools/Dev Tool Window")]
    private static void OpenWindow()
    {
        GetWindow<DevToolWindow>().Show();
    }

    #region TAB TIEN TE
    // --- TAB TIỀN TỆ (COIN, STAR & HEART) ---
    [TabGroup("DevToolTabs", "Currency")] 
    [Title("Amount Settings", titleAlignment: TitleAlignments.Centered)]
    [InfoBox("Số lượng tiền tệ sẽ được thêm vào khi click button")]
    public int amountIncrease = 10000;

    [TabGroup("DevToolTabs", "Currency")]
    [Title("Coin & Star Management", titleAlignment: TitleAlignments.Centered)]
    [Button("Increase Coin", ButtonSizes.Large)]
    [PropertyTooltip("Thêm số lượng Coin theo Amount Settings")]
    private void IncreaseCoin()
    {
        UseProfile.Coin += amountIncrease;
    }

    [TabGroup("DevToolTabs", "Currency")]
    [Button("Increase Star", ButtonSizes.Large)]
    [DetailedInfoBox("Increase Star", "Thêm số lượng Star vào tài khoản người chơi. Số lượng được xác định bởi giá trị 'amountIncrease' ở trên.")]
    private void IncreaseStar()
    {
        UseProfile.Star += amountIncrease;
    }

    [TabGroup("DevToolTabs", "Currency")]
    [HorizontalGroup("DevToolTabs/Currency/Resets")]
    [Button("Reset Coin", ButtonSizes.Medium)]
    [InfoBox("Đặt lại Coin về 0", InfoMessageType.Warning)]
    private void ResetCoin()
    {
        UseProfile.Coin = 0;
    }

    [HorizontalGroup("DevToolTabs/Currency/Resets")]
    [Button("Reset Star", ButtonSizes.Medium)]
    [InfoBox("Đặt lại Star về 0", InfoMessageType.Warning)]
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
        isUnlimitHeart = !isUnlimitHeart;
        UseProfile.IsUnlimitedHeart = isUnlimitHeart;
    }
    #endregion

    #region TAB THOI GIAN
    // --- TAB THỜI GIAN ---
    [TabGroup("DevToolTabs", "Time")]
    [Button("Reset Day (Yesterday)", ButtonSizes.Large)]
    [PropertyTooltip("Đặt thời gian đăng nhập cuối về hôm qua")]
    private void ResetDay()
    {
        UseProfile.TimeLastLoginDate = DateTime.Now.AddDays(-1);
    }

    [TabGroup("DevToolTabs", "Time")]
    [Button("Next Day", ButtonSizes.Large)]
    [PropertyTooltip("Giả lập việc người chơi đăng nhập vào ngày hôm sau")]
    private void NextDay()
    {
        UseProfile.TimeLastLoginDate = DateTime.Now.AddDays(-1);
    }

    [TabGroup("DevToolTabs", "Time")]
    [Button("Next 2 Days", ButtonSizes.Large)]
    [PropertyTooltip("Giả lập việc người chơi đăng nhập sau 2 ngày")]
    private void Next2Day()
    {
        UseProfile.TimeLastLoginDate = DateTime.Now.AddDays(-2);
    }
    #endregion

    #region TAB LEVEL
    // --- TAB LEVEL ---
    [TabGroup("DevToolTabs", "Level")] 
    [InfoBox("Số level tối đa muốn unlock")]
    public int maxLevelUnlock = 100;

    [TabGroup("DevToolTabs", "Level")]
    [Button("Max Unlock Level", ButtonSizes.Large)]
    [PropertyTooltip("Mở khóa tất cả level đến số lượng đã chỉ định")]
    private void MaxUnlockLevel()
    {
        UseProfile.MaxUnlockedLevel = maxLevelUnlock;
    }

    [TabGroup("DevToolTabs", "Level")]
    [Button("Next Level", ButtonSizes.Large)]
    [DetailedInfoBox("Next Level", "Chuyển sang level tiếp theo. Nếu đang ở level cuối thì sẽ tự động unlock thêm 1 level mới.")]
    private void NextLevel()
    {
        if (UseProfile.CurrentLevel == UseProfile.MaxUnlockedLevel)
        {
            UseProfile.MaxUnlockedLevel++;
        }
        UseProfile.CurrentLevel++;
    }

    [TabGroup("DevToolTabs", "Level")]
    [Button("Show Popup Win", ButtonSizes.Large)]
    [PropertyTooltip("Hiển thị popup chiến thắng để test UI")]
    private void ShowPopupWin()
    {
        WinBox.Setup().Show();
    }

    [TabGroup("DevToolTabs", "Level")]
    [Button("Show Popup Win Full", ButtonSizes.Large)]
    [PropertyTooltip("Hiển thị popup chiến thắng full progress")]
    private void ShowPopupWinFull()
    {
        UseProfile.LevelWinBoxProgress = 4;
        WinBox.Setup().Show();
    }
    
    #endregion

    #region TAB OTHER
    // --- TAB KHÁC ---
    [TabGroup("DevToolTabs", "Other")]
    [Title("Ads Management", titleAlignment: TitleAlignments.Centered)]
    [ReadOnly]
    public bool isRemoveAds;
    [TabGroup("DevToolTabs", "Other")]
    [Button("Toggle Remove Ads", ButtonSizes.Large)]
    private void RemoveAds()
    {
        isRemoveAds = !isRemoveAds;
        UseProfile.IsRemoveAds = isRemoveAds;
    }

    [TabGroup("DevToolTabs", "Other")]
    [Button("Next Scene Game", ButtonSizes.Large)]
    [PropertyTooltip("Chuyển sang scene gameplay")]
    private void NextSceneGame()
    {
        GameController.Instance.effectChangeScene.FadeToScene(SceneName.GAME_PLAY);
    }
    #endregion

    #region TAB LOCALIZATION
    // --- TAB LOCALIZATION ---
    [TabGroup("DevToolTabs", "Localization")]
    [Title("Paths and URL", titleAlignment: TitleAlignments.Centered)]
    [InfoBox("URL Google Sheets ở chế độ CSV export")]
    public string googleSheetUrl = "https://docs.google.com/spreadsheets/d/1QdVYWCZSUDpdcWH1JoGkIA7Svf1iWrXe4Rl06xYT-0g/export?format=csv&gid=633283787";

    [TabGroup("DevToolTabs", "Localization")]
    [InfoBox("Đường dẫn file CSV local để lưu backup")]
    public string localPath = "Assets/00_BaseGame/02_DataSO/CSV/localizationCSV.csv";

    [TabGroup("DevToolTabs", "Localization")]
    [InfoBox("Đường dẫn ScriptableObject LocalizationData")]
    public string soPath = "Assets/00_BaseGame/02_DataSO/LocalizationData.asset";

    [TabGroup("DevToolTabs", "Localization")]
    [Title("Import Actions", titleAlignment: TitleAlignments.Centered)]
    [Button("Import from Google Sheets", ButtonSizes.Large)]
    [DetailedInfoBox("Import from Google Sheets", "Tải dữ liệu từ Google Sheets, lưu backup vào file local và import vào ScriptableObject.")]
    private async void ImportFromGoogleSheets()
    {
        await LocalizationImporter.ImportFromGoogleSheets(googleSheetUrl, localPath, soPath);
    }

    [TabGroup("DevToolTabs", "Localization")]
    [Button("Import from Local File", ButtonSizes.Large)]
    [PropertyTooltip("Đọc file CSV local và import vào ScriptableObject")]
    private void ImportFromLocalFile()
    {
        LocalizationImporter.ImportFromLocalFile(localPath, soPath);
    }
    #endregion
    protected override void OnEnable()
    {
        base.OnEnable();
        isUnlimitHeart = UseProfile.IsUnlimitedHeart;
    }
    
}