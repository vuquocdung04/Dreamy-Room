using UnityEngine;

public class GameController : Singleton<GameController>
{
    public StartLoading startLoading;
    public MusicController musicController;
    public UseProfile useProfile;
    public DataContains dataContains;
    public AdmobController admobController;
    public EffectChangeScene effectChangeScene;
    public EffectChangeScene2 effectChangeScene2;
    public EffectController effectController;
    public HeartGame heartGame;
    public string curSceneName;
    public string curGameModeName;
    protected override void OnAwake()
    {
        Init();
    }

    private void Init()
    {
        Application.targetFrameRate = UseProfile.TargetFrameRate;
        admobController.Init();
        dataContains.Init();
        musicController.Init();
        effectChangeScene2.Init();
        heartGame.Init();
        startLoading.Init();
    }

    public void SetTargetFrameRate(int  tgFrameRate)
    {
        UseProfile.TargetFrameRate = tgFrameRate;
        Application.targetFrameRate = UseProfile.TargetFrameRate;
    }

    public void ChangeScene2(string sceneName)
    {
        effectChangeScene2.ChangeScene(sceneName);
        curSceneName = sceneName;
    }
    public void IncreaseLevel()
    {
        if (UseProfile.CurrentLevel == UseProfile.MaxUnlockedLevel)
        {
            UseProfile.MaxUnlockedLevel++;
        }
        UseProfile.CurrentLevel++;
    }

    public bool IsGameModeRelax()
    {
        return curGameModeName.Equals(GameMode.RELAX);
    }

    public bool IsGameModeNormal()
    {
        return curGameModeName.Equals(GameMode.NORMAL);
    }

    public bool IsSceneHome()
    {
        return curSceneName.Equals(SceneName.HOME_SCENE);
    }

    public bool IsSceneGamePlay()
    {
        return curSceneName.Equals(SceneName.GAME_PLAY);
    }
}


