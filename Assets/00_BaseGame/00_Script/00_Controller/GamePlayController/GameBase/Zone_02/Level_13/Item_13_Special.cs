using UnityEngine;


public class Item_13_Special : ItemBase, IPostPlacementAction
{
    public bool isInsideMask;
    public void HandlePostPlacementAction()
    {
        spriteRenderer.maskInteraction = isInsideMask ? SpriteMaskInteraction.VisibleInsideMask :  SpriteMaskInteraction.VisibleOutsideMask;
    }
}