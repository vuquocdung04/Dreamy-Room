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


    protected override void Init()
    {
        canvas.worldCamera = GamePlayController.Instance.playerContains.mainCamera;
        UpdateStateVib_Music_Sound();

        ActionBtnClick(btnClose, delegate
        {
            Close();
            GamePlayController.Instance.ResumeGame();
        });
        ActionBtnClick(btnGoHome, delegate
        {
            if (GameController.Instance.curGameModeName.Equals(GameMode.NORMAL))
                QuitLevelBox.Setup().Show();
            else
                GameController.Instance.ChangeScene2(SceneName.HOME_SCENE);
        });
        ActionBtnClick(btnRetry, () => { GameController.Instance.ChangeScene2(SceneName.GAME_PLAY); });
        ActionBtnClick(btnVib, () =>
        {
            bool newState = ToggleSetting(GameController.Instance.useProfile.OnVib, imgVib);
            GameController.Instance.useProfile.OnVib = newState;
        });
        ActionBtnClick(btnMusic, () =>
        {
            bool newState = ToggleSetting(GameController.Instance.useProfile.OnMusic, imgMusic);
            GameController.Instance.useProfile.OnMusic = newState;
        });
        ActionBtnClick(btnSound, () =>
        {
            bool newState = ToggleSetting(GameController.Instance.useProfile.OnSound, imgSound);
            GameController.Instance.useProfile.OnSound = newState;
        });
    }

    protected override void InitState()
    {
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
        var onVib = GameController.Instance.useProfile.OnVib;
        var onSound = GameController.Instance.useProfile.OnSound;
        var onMusic = GameController.Instance.useProfile.OnMusic;

        imgVib.sprite = onVib ? spriteOn : spriteOff;
        imgMusic.sprite = onMusic ? spriteOn : spriteOff;
        imgSound.sprite = onSound ? spriteOn : spriteOff;
    }
}