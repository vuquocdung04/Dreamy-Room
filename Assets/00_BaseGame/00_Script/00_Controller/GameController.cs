
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
        Application.targetFrameRate = 60;
        startLoading.Init();
        admobController.Init();
        dataContains.Init();
        musicController.Init();
    }
}
