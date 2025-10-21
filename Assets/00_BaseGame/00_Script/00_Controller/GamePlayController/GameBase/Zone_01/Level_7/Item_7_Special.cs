using UnityEngine;

public class Item_7_Special : ItemBase, IPostPlacementAction
{
    public bool isInside;
    public void HandlePostPlacementAction()
    {
        spriteRenderer.maskInteraction =
            isInside ? SpriteMaskInteraction.VisibleInsideMask : SpriteMaskInteraction.VisibleOutsideMask;
    }
}