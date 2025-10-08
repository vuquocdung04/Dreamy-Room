
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
        
        
        startLoading.Init();
    }

    public void SetTargetFrameRate(int  tgFrameRate)
    {
        UseProfile.TargetFrameRate = tgFrameRate;
        Application.targetFrameRate = UseProfile.TargetFrameRate;
    }

    public void ChangeScene2(string sceneName)
    {
        effectChangeScene2.RunEffect(sceneName);
    }
}
