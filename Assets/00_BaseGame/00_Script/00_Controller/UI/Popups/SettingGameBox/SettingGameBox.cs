using UnityEngine;
using UnityEngine.UI;

public class SettingGameBox : BoxSingleton<SettingGameBox>
{
    public static SettingGameBox Setup()
    {
        return Path(PathPrefabs.SETTING_GAME_BOX);
    }

    public Canvas canvas;

    public Button btnClose;
    public Button btnGoHome;
    public Button btnRetry;
    [Space(5)] public Sprite spriteOn;
    public Sprite spriteOff;
    [Space(5)] public Button btnVib;
    public Button btnSound;
    public Button btnMusic;

    public Image imgVib;
    public Image imgSound;
    public Image imgMusic;

    public LocalizedText lcTitle;
    private GameController gameController;
    private UseProfile useProfile;
    
    protected override void Init()
    {
        canvas.worldCamera = GamePlayController.Instance.playerContains.mainCamera;
        gameController = GameController.Instance;
        useProfile = GameController.Instance.useProfile;
        UpdateStateVib_Music_Sound();

        ActionBtnClick(btnClose, delegate
        {
            Close();
            GamePlayController.Instance.ResumeGame();
        });
        ActionBtnClick(btnGoHome, delegate
        {
            if (gameController.IsGameModeNormal())
                QuitLevelBox.Setup().Show();
            else
                gameController.ChangeScene2(SceneName.HOME_SCENE);
        });
        ActionBtnClick(btnRetry, () => { gameController.ChangeScene2(SceneName.GAME_PLAY); });
        ActionBtnClick(btnVib, () =>
        {
            bool newState = ToggleSetting(useProfile.OnVib, imgVib);
            GameController.Instance.useProfile.OnVib = newState;
        });
        ActionBtnClick(btnMusic, () =>
        {
            bool newState = ToggleSetting(useProfile.OnMusic, imgMusic);
            useProfile.OnMusic = newState;
        });
        ActionBtnClick(btnSound, () =>
        {
            bool newState = ToggleSetting(useProfile.OnSound, imgSound);
            useProfile.OnSound = newState;
        });
        lcTitle.Init();
    }

    protected override void InitState()
    {
        RefreshLocalization(gameController.dataContains.DataPlayer, InitLocalization);
    }

    private void InitLocalization()
    {
        lcTitle.Init();
    }

    private void ActionBtnClick(Button btn, System.Action callback = null)
    {
        btn.onClick.AddListener(delegate { callback?.Invoke(); });
    }

    private bool ToggleSetting(bool currentValue, Image img)
    {
        bool newValue = !currentValue;
        img.sprite = newValue ? spriteOn : spriteOff;
        return newValue;
    }

    private void UpdateStateVib_Music_Sound()
    {
        var onVib = useProfile.OnVib;
        var onSound = useProfile.OnSound;
        var onMusic = useProfile.OnMusic;

        imgVib.sprite = onVib ? spriteOn : spriteOff;
        imgMusic.sprite = onMusic ? spriteOn : spriteOff;
        imgSound.sprite = onSound ? spriteOn : spriteOff;
    }
}