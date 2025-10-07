using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public LevelBase currentLevel;
    public List<Color> lsColors;

    public void Init()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        var dataLevel = GameController.Instance.dataContains.dataLevel;
        GameObject levelPrefab = null;
        levelPrefab = Instantiate(!UseProfile.HasCompletedLevelTutorial
            ? dataLevel.levelTutorial
            : dataLevel.GetLevelPrefabById(UseProfile.CurrentLevel));

        if (levelPrefab == null)
        {
            Debug.Log("Fail Generate Level");
            return;
        }
        Debug.Log("Completed Generate Level");
        currentLevel = levelPrefab.GetComponent<LevelBase>();
        currentLevel.Init();
        int rand = Random.Range(0, lsColors.Count);
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