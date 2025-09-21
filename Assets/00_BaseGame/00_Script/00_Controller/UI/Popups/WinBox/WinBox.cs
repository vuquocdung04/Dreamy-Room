
using UnityEngine;
using UnityEngine.UI;

public class WinBox : BoxSingleton<WinBox>
{
    public static WinBox Setup()
    {
        return Path(PathPrefabs.WIN_BOX);
    }
    protected override void Init()
    {
    }

    protected override void InitState()
    {
    }
}
