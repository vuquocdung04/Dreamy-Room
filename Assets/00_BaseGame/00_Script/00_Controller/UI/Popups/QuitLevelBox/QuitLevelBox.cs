using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuitLevelBox : BoxSingleton<QuitLevelBox>
{
    public static QuitLevelBox Setup()
    {
        return Path(PathPrefabs.QUIT_LEVEL_BOX);
    }

    public Button btnClose;
    public Button btnCloseByPanel;
    public Button btnQuit;

    public TextMeshProUGUI txtHeart;
    public TextMeshProUGUI txtStar;
    
    protected override void Init()
    {
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
        //NOTE: GO TO HOME SCENE AND DEDUCE HEART AND STAR
        GameController.Instance.effectChangeScene2.RunEffect(SceneName.HOME_SCENE);
    }
}