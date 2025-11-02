using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public LevelBase currentLevel;
    public Sprite sprBgTut;
    public List<Color> lsColors;
    public Image imgBg;
    public Image imgPattern;
    private int idLevel;
    public void Init()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        var dataLevel = GameController.Instance.dataContains.dataLevel;
        idLevel = UseProfile.CurrentLevel;
        bool hasCompletedLevelTut = UseProfile.HasCompletedLevelTutorial;
        
        var levelPrefab = Instantiate(!hasCompletedLevelTut
            ? dataLevel.levelTutorial
            : dataLevel.GetLevelPrefabById(idLevel));

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
        SetSpriteBg(dataLevel.levelTutorial);
    }

    private void SetSpriteBg(bool isTut)
    {
        imgBg.gameObject.SetActive(true);
        var gameController = GameController.Instance;
        imgBg.sprite = !isTut ? sprBgTut : gameController.dataContains.dataLevel.GetBgSpriteById(idLevel);
        
        var pattern = gameController.dataContains.dataLevel.GetPatternById(idLevel);
        if(pattern == null) return;
        imgPattern.gameObject.SetActive(true);
        imgPattern.sprite = pattern;
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