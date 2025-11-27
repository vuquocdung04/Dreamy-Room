using UnityEngine;

public class GameController : Singleton<GameController>
{
    public StartLoading startLoading;
    public UseProfile useProfile;
    public DataContains dataContains;
    public AdmobController admobController;
    public EffectChangeScene effectChangeScene;
    public EffectChangeScene2 effectChangeScene2;
    public EffectController effectController;
    public HeartGame heartGame;
    public LocalizationController localizationController;
    public AudioController audioController;
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
        
        audioController.Init();
        effectChangeScene2.Init();
        heartGame.Init();
        localizationController.Init();
        startLoading.Init();
    }

    public void SetTargetFrameRate(int tgFrameRate)
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


    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Debug.Log("OnApplicationPause: Game is pausing. Saving data...");
            dataContains.SaveData();
        }
    }
    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit: Game is quitting. Saving data...");
        if (dataContains != null)
        {
            dataContains.SaveData();
        }
    }
}