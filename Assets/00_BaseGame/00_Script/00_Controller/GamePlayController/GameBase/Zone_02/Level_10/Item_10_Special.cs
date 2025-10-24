using UnityEngine;

public class Item_10_Special : ItemBase, IPostPlacementAction
{
    public void HandlePostPlacementAction()
    {
        spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }
}