using UnityEngine;

public class Item_6_Bear : ItemBase, IPostPlacementAction
{
    public void HandlePostPlacementAction()
    {
        gameObject.SetActive(false);
        var level6 = (Level_6)GamePlayController.Instance.levelController.currentLevel;
        level6.HandleBearAnimation();
    }
}