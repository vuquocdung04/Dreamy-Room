using System.Linq;
using UnityEngine;

public class Item_10_InTheWardore : ItemBase, IPostPlacementAction
{
    [SerializeField] private bool isInsideMask;
    [SerializeField] private bool isWardrobeOpen;

    public static System.Action postEvent;
    public void SetIsDoorOpened(bool state)  => this.isWardrobeOpen = state;
    public override void ValidateUnlockState()
    {
        if (isAvailableForHint) return;
        if (slotsSnap == null) return;

        isAvailableForHint = slotsSnap.All(slot => slot != null && slot.IsReadyToReceiveItem() && isWardrobeOpen);
    }

    public void HandlePostPlacementAction()
    {
        spriteRenderer.maskInteraction = isInsideMask ? SpriteMaskInteraction.VisibleInsideMask : SpriteMaskInteraction.VisibleOutsideMask;
        postEvent?.Invoke();
    }
}