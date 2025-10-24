using System.Linq;
using UnityEngine;

    public class Slot_10_InTheWardore : ItemSlot
    {
        [SerializeField] private bool isDoorOpened;
        public void SetIsDoorOpened(bool isOpen) => this.isDoorOpened = isOpen;
        public override void ValidateReadyState()
        {
            bool allConditionsMet = conditionSlots.All(slot => slot.isFullSlot) && isDoorOpened;
            if (allConditionsMet)
            {
                isReadyShow = true;
            }
        }

        public override bool IsAvailableForMagicWand()
        {
            return base.IsAvailableForMagicWand() && isDoorOpened;
        }
    }