using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public LevelBase currentLevel;
    public List<Color> lsColors;
    public List<Sprite> lsSpritesBg;
    public Image imgBg;

    public void Init()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        var dataLevel = GameController.Instance.dataContains.dataLevel;
        var levelPrefab = Instantiate(!UseProfile.HasCompletedLevelTutorial
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
        
        RandomSpriteBg();
    }

    private void RandomSpriteBg()
    {
        int rand = Random.Range(0, lsSpritesBg.Count);
        imgBg.sprite = lsSpritesBg[rand];
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