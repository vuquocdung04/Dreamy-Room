using System.Linq;
using UnityEngine;

    public class Slot_10_InTheWardore : ItemSlot
    {
        [SerializeField] private bool isWardrobeOpen;
        public void SetIsDoorOpened(bool isOpen) => isWardrobeOpen = isOpen;
        
        
        public override bool IsReadyToReceiveItem()
        {
            if (!isWardrobeOpen)
            {
                return false;
            }
            return base.IsReadyToReceiveItem();
        }
        
        public override void ValidateReadyState()
        {
            bool allConditionsMet = conditionSlots.All(slot => slot.isFullSlot);
            if (allConditionsMet && isWardrobeOpen)
            {
                isReadyShow = true;
            }
        }

        public override bool IsAvailableForMagicWand()
        {
            return base.IsAvailableForMagicWand() && isWardrobeOpen;
        }
    }