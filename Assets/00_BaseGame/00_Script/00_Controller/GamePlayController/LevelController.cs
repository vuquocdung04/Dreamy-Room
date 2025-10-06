
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public LevelBase currentLevel;
    public Color color = Color.yellow;
    public void Init()
    {
        currentLevel.Init();
        currentLevel.SetColorBox(color);
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

    public void GenerateLevel()
    {
        currentLevel.SetColorBox(color);
    }
}