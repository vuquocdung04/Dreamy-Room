
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public StartLoading startLoading;
    public MusicController musicController;
    public UseProfile useProfile;
    public DataContains dataContains;
    public AdmobController admobController;
    public EffectChangeScene effectChangeScene;
    protected override void OnAwake()
    {
        Init();
    }

    private void Init()
    {
        Application.targetFrameRate = UseProfile.TargetFrameRate;
        startLoading.Init();
        admobController.Init();
        dataContains.Init();
        musicController.Init();
    }

    public void SetTargetFrameRate(int  tgFrameRate)
    {
        UseProfile.TargetFrameRate = tgFrameRate;
        Application.targetFrameRate = UseProfile.TargetFrameRate;
    }
}
