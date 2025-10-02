using UnityEngine;

public class LevelController : MonoBehaviour
{
    public LevelBase currentLevel;


    public void Init()
    {
        currentLevel.Init();
    }

    public bool HasItemOutOfBox()
    {
        return currentLevel.HasItemOutOfBox();
    }

    public bool HasReadyShadows()
    {
        return currentLevel.HasReadyShadowsForMagicWand();
    }
}