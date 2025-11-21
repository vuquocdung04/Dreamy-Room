using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Item_2_Shower : ItemBase, IPostPlacementAction
{
    public void HandlePostPlacementAction()
    {
        var level_2 =
            (Level_2)GamePlayController.Instance.levelController.currentLevel;
        level_2.HandleBathFillWater();
    }
}