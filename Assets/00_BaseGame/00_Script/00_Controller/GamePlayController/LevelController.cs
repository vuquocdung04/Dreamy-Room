
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public LevelBase currentLevel;
    public List<Color> lsColors;
    public void Init()
    {
        currentLevel.Init();
        int rand =  Random.Range(0, lsColors.Count);
        currentLevel.SetColorBox(lsColors[rand]);
    }
    

    public bool HasItemOutOfBox()
    {
        return currentLevel.HasItemOutOfBox();
    }

    public bool HasReadyShadows()
    {
        return currentLevel.HasReadyShadowsForMagicWand();
    }
    
    public void SetBoxReadyForInteraction(bool ready)
    {
        currentLevel.SetBoxReadyForInteraction(ready);
    }

}