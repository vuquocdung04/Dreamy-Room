using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Item_2_Shower : ItemBase, IPostPlacementAction
{
    public List<SpriteRenderer> lsSpriteRenderers;
    public void HandlePostPlacementAction()
    {
        foreach (var spr in lsSpriteRenderers)
        {
            spr.DOFade(1f, 0.75f).SetEase(Ease.Linear);
        }
    }
    
}