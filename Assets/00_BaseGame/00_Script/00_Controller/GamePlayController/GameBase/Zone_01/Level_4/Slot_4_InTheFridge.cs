using System.Linq;
using UnityEngine;

public class Slot_4_InTheFridge : Slot_4
{
    [SerializeField] private bool isDoorOpened;
    public void SetActiveWhenOpened() => isDoorOpened = true;
    public override bool IsAvailableForMagicWand()
    {
        return base.IsAvailableForMagicWand() && isDoorOpened;
    }

    public override void ValidateReadyState()
    {
        bool allConditionsMet = conditionSlots.All(slot => slot.isFullSlot);
        if (allConditionsMet && isDoorOpened)
        {
            isReadyShow = true;
        }
    }
}