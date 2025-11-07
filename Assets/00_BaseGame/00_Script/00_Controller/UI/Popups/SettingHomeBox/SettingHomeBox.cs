using UnityEngine;
using UnityEngine.UI;

public class SettingHomeBox : BoxSingleton<SettingHomeBox>
{
    public static SettingHomeBox Setup()
    {
        return Path(PathPrefabs.SETTING_HOME_BOX);
    }

    public Button btnClose;
    [Space(5)]
    public Button btnVib;
    public Button btnMusic;
    public Button btnSound;
    public Image imgInactiveVib;
    public Image imgInactiveMusic;
    public Image imgInactiveSound;
    [Space(5)]
    public Button btnChooseLanguage;
    [Space(5)]
    public Button btnFps30;
    public Button btnFps90;
    public Button btnFps120;
    public Image imgFps30;
    public Image imgFps90;
    public Image imgFps120;
    [Space(5)]
    public Button btnGoToFbFanpage;
    public Button btnGoToFbGroup;
    public Button btnGoToTikTok;
    public Button btnMail;
    [Header("Localization")]
    public LocalizedText lcTitle;
    public LocalizedText lcCurLanguage;
    private int curFrameRate;
    
    protected override void Init()
    {
        UpdateStateVib_Music_Sound();
        UpdateStateFps();
        
        btnClose.onClick.AddListener(Close);
        
        btnVib.onClick.AddListener(delegate
        {
            bool newState = ToggleSetting(GameController.Instance.useProfile.OnVib, imgInactiveVib);
            GameController.Instance.useProfile.OnVib = newState;
        });
        btnMusic.onClick.AddListener(delegate
        {
            bool newState = ToggleSetting(GameController.Instance.useProfile.OnMusic, imgInactiveMusic);
            GameController.Instance.useProfile.OnMusic = newState;
        });
        btnSound.onClick.AddListener(delegate
        {
            bool newState = ToggleSetting(GameController.Instance.useProfile.OnSound, imgInactiveSound);
            GameController.Instance.useProfile.OnSound = newState;
        });
        
        btnChooseLanguage.onClick.AddListener(delegate
        {
            LocalizationBox.Setup().Show();
        });
        
        btnFps30.onClick.AddListener(delegate
        {
            OnFpsButtonClicked(30);
            
        });
        
        btnFps90.onClick.AddListener(delegate
        {
            OnFpsButtonClicked(90);
        });
        
        btnFps120.onClick.AddListener(delegate
        {
            OnFpsButtonClicked(120);

        });
        
        btnMail.onClick.AddListener(delegate
        {
            Close();
        });
        InitLocalization();
    }

    protected override void InitState()
    {
        
    }

    private void InitLocalization()
    {
        lcTitle.Init();
        lcCurLanguage.Init();
    }
    
    private bool ToggleSetting(bool currentValue, Image inactiveImage)
    {
        bool newValue = !currentValue;
        inactiveImage.gameObject.SetActive(!newValue);
        return newValue;
    }
    
    private void OnFpsButtonClicked(int newFrameRate)
    {
        int currentFrameRate = UseProfile.TargetFrameRate;
        if (currentFrameRate == newFrameRate)
        {
            return; 
        }
        GameController.Instance.SetTargetFrameRate(newFrameRate);
        UpdateStateFps();
    }
    
    private void UpdateStateVib_Music_Sound()
    {
        var onVib = GameController.Instance.useProfile.OnVib;
        var onSound = GameController.Instance.useProfile.OnSound;
        var onMusic = GameController.Instance.useProfile.OnMusic;

        imgInactiveVib.gameObject.SetActive(!onVib);
        imgInactiveSound.gameObject.SetActive(!onSound);
        imgInactiveMusic.gameObject.SetActive(!onMusic);
    }

    private void UpdateStateFps()
    {
        var curFps = UseProfile.TargetFrameRate;
        imgFps30.gameObject.SetActive(curFps == 30);
        imgFps90.gameObject.SetActive(curFps == 90);
        imgFps120.gameObject.SetActive(curFps == 120);
    }
}