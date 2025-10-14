

public class GamePlayController : Singleton<GamePlayController>
{
    public GameScene gameScene;
    public PlayerContains playerContains;
    public LevelController levelController;
    public EffectBoosterController effectBoosterController;
    public bool IsWin {get; private set; }
    protected override void OnAwake()
    {
        base.OnAwake();
        m_DontDestroyOnLoad = false;
        Init();
    }
    
    private void Init()
    {
        gameScene.Init();
        levelController.Init();
        playerContains.Init();
        effectBoosterController.Init();
        
        HandleUnlockBar();
        HandleUnlockCamera();
    }

    private void HandleUnlockCamera()
    {
        var maxLevel = UseProfile.MaxUnlockedLevel;
        playerContains.inputManager.SetCanMoveCamera(maxLevel >= 5);
    }
    
    private void HandleUnlockBar()
    {
        var maxLevel = UseProfile.MaxUnlockedLevel;
        
        if (!UseProfile.HasCompletedLevelTutorial)
        {
            gameScene.HideBottomBar();
            gameScene.HideTopBar();
        }
        else
        {
            if(maxLevel < 2)
                gameScene.HideBoosterBar();
        }
    }

    public void LoseGame()
    {
        playerContains.inputManager.SetLose(true);
    }
    public void WinGame()
    {
        IsWin = true;
        playerContains.inputManager.SetWin(true);
        gameScene.HideBottomBar();
        gameScene.HideTopBar();
    }
    
    public void PauseGame()
    {
        gameScene.PauseTime();   
        playerContains.PauseGame(true);
    }

    public void ResumeGame()
    {
        gameScene.ResumeTime();
        playerContains.PauseGame(false);
    }
}
