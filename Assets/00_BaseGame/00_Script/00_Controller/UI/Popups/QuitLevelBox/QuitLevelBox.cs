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
    }

    protected override void InitState()
    {
    }

    private void OnClickQuit()
    {
        var gameController = GameController.Instance;
        gameController.ChangeScene2(SceneName.HOME_SCENE);
        gameController.heartGame.TryUseHeart();
    }

}