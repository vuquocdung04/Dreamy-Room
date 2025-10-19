using UnityEngine;

public class Item_5_Special : ItemBase, IPostPlacementAction
{
    public bool isInsideMask = true;
    public void HandlePostPlacementAction()
    {
        spriteRenderer.maskInteraction = isInsideMask ? SpriteMaskInteraction.VisibleInsideMask : SpriteMaskInteraction.VisibleOutsideMask;
    }
}