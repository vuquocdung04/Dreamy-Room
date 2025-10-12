

public class GamePlayController : Singleton<GamePlayController>
{
    public GameScene gameScene;
    public PlayerContains playerContains;
    public LevelController levelController;
    public EffectController effectController;
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
        effectController.Init();
        
        HandleUnlockBar();
        HandleUnlockCamera();
    }

    private void HandleUnlockCamera()
    {
        var maxLevel = UseProfile.MaxUnlockedLevel;
        if(maxLevel >= 3)
            playerContains.inputManager.SetCanMoveCamera(true);
        else
            playerContains.inputManager.SetCanMoveCamera(false);
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
