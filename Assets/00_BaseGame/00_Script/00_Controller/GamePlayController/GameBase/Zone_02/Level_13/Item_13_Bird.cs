using UnityEngine;

public class Item_13_Bird : ItemBase, IPostPlacementAction
{
    public bool isBlue;
    public void HandlePostPlacementAction()
    {
        var level_13 = (Level_13)GamePlayController.Instance.levelController.currentLevel;
        gameObject.SetActive(false);
        if(isBlue)
            level_13.BirdBlueAnim();
        else
            level_13.BirdPinkAnim();
    }
}