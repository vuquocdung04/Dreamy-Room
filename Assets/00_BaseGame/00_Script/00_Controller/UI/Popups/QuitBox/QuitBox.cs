using UnityEngine;

public class QuitBox : BoxSingleton<QuitBox>
{
    public static QuitBox Setup()
    {
        return Path(PathPrefabs.QUIT_BOX);
    }
    protected override void Init()
    {
    }

    protected override void InitState()
    {
    }
}