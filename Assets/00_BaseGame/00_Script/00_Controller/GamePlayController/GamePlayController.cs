

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
             HandleUnlockCamera();
             HandleUnlockBar();
             ResumeGame();
        });
    }
    
    private void HandleUnlockCamera()
    {
        var maxLevel = UseProfile.MaxUnlockedLevel;
        var currentLevel = UseProfile.CurrentLevel;
        
        bool hasUnlockedFeature = maxLevel > 5;
        
        bool isPreviewingOnLevel5 = (currentLevel == 5 && maxLevel == 5);
        
        playerContains.inputManager.SetCanMoveCamera(hasUnlockedFeature || isPreviewingOnLevel5);
    }
    
    private void HandleUnlockBar()
    {
        var maxLevel = UseProfile.MaxUnlockedLevel;
        var currentLevel = UseProfile.CurrentLevel;
        var isNormalMode = GameController.Instance.IsGameModeNormal();
        if (!UseProfile.HasCompletedLevelTutorial)
        {
            //Note: hide all roi
        }
        else
        {
            gameScene.DisplayTopBar();
            gameScene.DisplayBottomBar();

            if (isNormalMode)
            {
                gameScene.DisplayProgressBar();
                if(maxLevel >= 2)
                    gameScene.DisplayBoosterBar();
            }
            
            if (currentLevel == 5 && maxLevel == 5)
            {
                gameScene.DisplaySwipeCam();
            }
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
