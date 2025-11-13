using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuitLevelBox : BoxSingleton<QuitLevelBox>
{
    public static QuitLevelBox Setup()
    {
        return Path(PathPrefabs.QUIT_LEVEL_BOX);
    }
    public Canvas canvas;
    public Button btnClose;
    public Button btnCloseByPanel;
    public Button btnQuit;

    public TextMeshProUGUI txtHeart;
    public TextMeshProUGUI txtStar;

    public LocalizedText lcTitle;
    public LocalizedText lcDesc;
    public LocalizedText lcQuit;
    
    protected override void Init()
    {
        canvas.worldCamera = GamePlayController.Instance.playerContains.mainCamera;
        btnClose.onClick.AddListener(Close);
        btnCloseByPanel.onClick.AddListener(Close);
        btnQuit.onClick.AddListener(delegate
        {
            Debug.Log("GOTO HOME SCENE && DEDUCE HEART AND STAR");
            OnClickQuit();
        });
        
        InitLocalization();
    }

    protected override void InitState()
    {
        RefreshLocalization(GameController.Instance.dataContains.DataPlayer, InitLocalization);
    }

    private void InitLocalization()
    {
        lcDesc.Init();
        lcQuit.Init();
        lcTitle.Init();
    }

    private void OnClickQuit()
    {
        var gameController = GameController.Instance;
        gameController.ChangeScene2(SceneName.HOME_SCENE);
        gameController.heartGame.TryUseHeart();
    }
}