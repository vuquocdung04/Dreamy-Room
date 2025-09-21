using UnityEngine;

public class TimeOutBox : BoxSingleton<TimeOutBox>
{
    public static TimeOutBox Setup()
    {
        return Path(PathPrefabs.TIME_OUT_BOX);
    }
    protected override void Init()
    {
    }

    protected override void InitState()
    {
    }
}