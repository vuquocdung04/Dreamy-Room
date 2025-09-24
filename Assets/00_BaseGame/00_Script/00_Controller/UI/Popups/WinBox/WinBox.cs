
using UnityEngine;
using UnityEngine.UI;

public class WinBox : BoxSingleton<WinBox>
{
    public static WinBox Setup()
    {
        return Path(PathPrefabs.WIN_BOX);
    }

    public Image imgFill;
    public Button btnNext;
    protected override void Init()
    {
        btnNext.onClick.AddListener(OnClickNext);
    }

    protected override void InitState()
    {
    }

    private void OnClickNext()
    {
        
    }
}
