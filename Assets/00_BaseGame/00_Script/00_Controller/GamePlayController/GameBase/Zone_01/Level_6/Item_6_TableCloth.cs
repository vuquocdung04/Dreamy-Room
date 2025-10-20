using System.Linq;
using UnityEngine;

public class Item_6_TableCloth : Item_6
{
    protected override void CheckItemPlacement(float threshold)
    {
        if (slotsSnap == null || slotsSnap.Count == 0)
        {
            OnFailSnap();
            return;
        }

        ItemSlot bestSlot = null;
        float minDistance = float.MaxValue;
        foreach (var slot in slotsSnap)
        {
            if(slot == null || slot.isFullSlot) continue;
            float distance = Vector2.Distance(transform.position, slot.transform.position);
            if (distance <= minDistance)
            {
                minDistance = distance;
                bestSlot = slot;
            }
        }
        if (bestSlot != null && minDistance <= threshold)
        {
            bool slotConditionsMet = true; 
            if (bestSlot.conditionSlots != null && bestSlot.conditionSlots.Count > 0)
            {
                slotConditionsMet = bestSlot.conditionSlots.All(slot => slot != null && slot.isFullSlot);
            }
            
            bool slotIsEmpty = !bestSlot.isFullSlot;
            
            if (slotConditionsMet && slotIsEmpty)
            {
                OnDoneSnap(bestSlot);
            }
            else
            {
                OnFailSnap();
            }
        }
        else
        {
            OnFailSnap();
        }
    }
}