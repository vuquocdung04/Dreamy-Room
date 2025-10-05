using UnityEngine;
public class TeamBox : BoxSingleton<TeamBox>
{
    public Canvas canvas;
    public static TeamBox Setup()
    {
         return Path(PathPrefabs.TEAM_BOX);
    }
    protected override void Init()
    {
        canvas.worldCamera = Camera.main;
    }

    protected override void InitState()
    {
        
    }
}