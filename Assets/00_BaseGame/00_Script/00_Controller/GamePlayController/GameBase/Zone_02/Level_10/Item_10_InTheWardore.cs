using System.Linq;
using UnityEngine;

public class Item_10_InTheWardore : ItemBase, IPostPlacementAction
{
    [SerializeField] private bool isInsideMask;
    [SerializeField] private bool isDoorOpened;

    public static System.Action postEvent;
    public void SetIsDoorOpened(bool state)  => this.isDoorOpened = state;
    public override void ValidateUnlockState()
    {
        if (isUnlocked) return;
        if (slotsSnap == null) return;

        isUnlocked = slotsSnap.All(slot => slot != null && slot.IsReadyToReceiveItem()) && isDoorOpened;
    }

    public void HandlePostPlacementAction()
    {
        spriteRenderer.maskInteraction = isInsideMask ? SpriteMaskInteraction.VisibleInsideMask : SpriteMaskInteraction.VisibleOutsideMask;
        postEvent?.Invoke();
    }
}