using UnityEngine;

public class SettingGameBox : BoxSingleton<SettingGameBox>
{
    public static SettingGameBox Setup()
    {
        return Path(PathPrefabs.SETTING_GAME_BOX);
    }
    protected override void Init()
    {
    }

    protected override void InitState()
    {
    }
}