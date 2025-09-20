using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class HomeBox : BoxSingleton<HomeBox>
{
    public static HomeBox Setup()
    {
        return Setup(PathPrefabs.HOME_BOX);
    }
    protected override void Init()
    {
        
    }

    protected override void InitState()
    {
        
    }
}