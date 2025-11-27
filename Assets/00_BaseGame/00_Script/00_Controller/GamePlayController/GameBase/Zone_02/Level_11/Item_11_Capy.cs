using UnityEngine;

public class Item_11_Capy : ItemBase, IPostPlacementAction
{
    public void HandlePostPlacementAction()
    {
        var level_11 = (Level_11)GamePlayController.Instance.levelController.currentLevel;
        gameObject.SetActive(false);
        level_11.HandleAnimationCapy();
    }
}