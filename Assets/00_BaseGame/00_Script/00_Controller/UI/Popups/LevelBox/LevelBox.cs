
using UnityEngine.UI;

public class LevelBox : BoxSingleton<LevelBox>
{
    public static LevelBox Setup()
    {
        return Path(PathPrefabs.LEVEL_BOX);
    }

    public Button btnClose;
    public Button btnCloseByPanel;
    public Button btnOk;
    public LocalizedText lcDesc;
    public LocalizedText lcOk;
    
    protected override void Init()
    {
        InitLocalization();
        btnClose.onClick.AddListener(Close);
        btnOk.onClick.AddListener(Close);
        btnCloseByPanel.onClick.AddListener(Close);
    }

    protected override void InitState()
    {
        RefreshLocalization(GameController.Instance.dataContains.DataPlayer,InitLocalization);
    }

    private void InitLocalization()
    {
        lcDesc.Init();
        lcOk.Init();
    }
}