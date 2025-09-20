using UnityEngine;

public class TeamBox : BoxSingleton<TeamBox>
{
    public static TeamBox Setup()
    {
         return Setup(PathPrefabs.TEAM_BOX);
    }
    protected override void Init()
    {
    }

    protected override void InitState()
    {
    }
}