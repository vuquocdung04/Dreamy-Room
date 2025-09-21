using UnityEngine;

public class SettingHomeBox : BoxSingleton<SettingHomeBox>
{
    public static SettingHomeBox Setup()
    {
        return Path(PathPrefabs.SETTING_HOME_BOX);
    }
    protected override void Init()
    {
    }

    protected override void InitState()
    {
    }
}