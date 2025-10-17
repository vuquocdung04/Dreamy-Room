

using DG.Tweening;

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
        
        PrevPlayGame();
    }
    private void PrevPlayGame()
    {
        playerContains.mainCamera.orthographicSize = 15f;
        gameScene.HideAllBar();
        levelController.currentLevel.HideBox();
    }
    public void InitEffect()
    {
        var targetSize = playerContains.cameraController.GetLimitSize();
        playerContains.mainCamera.DOOrthoSize(targetSize,0.5f).OnComplete(delegate
        {
             levelController.currentLevel.InitStateBox();
             HandleUnlockBar();
             HandleUnlockCamera();
             ResumeGame();
        });
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
            //Note: hide all roi
        }
        else
        {
            gameScene.DisplayTopBar();
            gameScene.DisplayBottomBar();
            if(maxLevel < 2)
                gameScene.DisplayBoosterBar();
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
        gameScene.HideAllBar();
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
