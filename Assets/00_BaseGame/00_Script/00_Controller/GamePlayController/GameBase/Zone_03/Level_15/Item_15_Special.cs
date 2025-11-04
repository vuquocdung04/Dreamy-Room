using UnityEngine;

public class Item_15_Special : ItemBase, IPostPlacementAction
{
    public bool isInsideMask;

    public void HandlePostPlacementAction()
    {
        spriteRenderer.maskInteraction = isInsideMask ? SpriteMaskInteraction.VisibleInsideMask  : SpriteMaskInteraction.VisibleOutsideMask;
    }
}