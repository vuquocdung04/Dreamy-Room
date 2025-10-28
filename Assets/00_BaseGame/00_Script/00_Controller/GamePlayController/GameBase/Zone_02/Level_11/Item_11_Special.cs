using UnityEngine;

public class Item_11_Special : ItemBase, IPostPlacementAction
{
    public void HandlePostPlacementAction()
    {
        spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
    }
}