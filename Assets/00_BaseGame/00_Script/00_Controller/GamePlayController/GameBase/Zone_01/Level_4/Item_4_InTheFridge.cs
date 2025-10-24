
using System.Linq;
using UnityEngine;

public class Item_4_InTheFridge : Item_4, IPostPlacementAction
{
    public static System.Action postEvent;
    [SerializeField] private bool isShelfItem = true;
    [SerializeField] private bool isDoorOpened;
    public void SetDoorOpened() => isDoorOpened = true;
    public override void ValidateUnlockState()
    {
        if (isAvailableForHint) return;
        if (slotsSnap == null) return;

        isAvailableForHint = slotsSnap.All(slot => slot != null && slot.IsReadyToReceiveItem() && isDoorOpened);
    }

    public void HandlePostPlacementAction()
    {
        if (isShelfItem)
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        
        postEvent?.Invoke();
    }
}