using UnityEngine;

public class Item_11_Birds : ItemBase, IPostPlacementAction
{
    public bool isBird1;
    public void HandlePostPlacementAction()
    {
        var level_11 = (Level_11)GamePlayController.Instance.levelController.currentLevel;
        gameObject.SetActive(false);
        if (isBird1)
        {
            level_11.HandleAnimationBird1();
        }
        else
        {
            level_11.HandleAnimationBird2();
        }
    }
}