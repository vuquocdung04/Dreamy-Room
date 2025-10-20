using UnityEngine;

public class Item_6_Special : ItemBase, IPostPlacementAction
{
    public void HandlePostPlacementAction()
    {
        spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
    }
}