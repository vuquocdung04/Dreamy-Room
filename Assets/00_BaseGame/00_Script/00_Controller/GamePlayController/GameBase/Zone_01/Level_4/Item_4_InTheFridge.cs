using System.Collections.Generic;
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
        if (isUnlocked) return;

        if (conditionSlots == null || conditionSlots.Count == 0) return;

        bool allConditionsMet = conditionSlots.All(slot => slot != null && slot.isFullSlot);

        if (allConditionsMet && isDoorOpened)
            isUnlocked = true;
    }

    public void HandlePostPlacementAction()
    {
        if (isShelfItem)
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        
        postEvent?.Invoke();
    }
}