using UnityEngine;

public class Item_9_Special : ItemBase, IPostPlacementAction
{
    public void HandlePostPlacementAction()
    {
        spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
    }
}