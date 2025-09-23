using UnityEngine;
using UnityEngine.UI;

public class LevelBox : BoxSingleton<LevelBox>
{
    public static LevelBox Setup()
    {
        return Path(PathPrefabs.LEVEL_BOX);
    }

    public Button btnClose;
    public Button btnOk;
    
    protected override void Init()
    {
        btnClose.onClick.AddListener(Close);
        btnOk.onClick.AddListener(Close);
    }

    protected override void InitState()
    {
    }
}