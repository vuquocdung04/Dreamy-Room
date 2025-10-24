
using System.Collections.Generic;
using UnityEngine;

public class L10_WardoreDoorOpen : MonoBehaviour
{
    public List<Item_10_InTheWardore> lsItems;
    public List<Slot_10_InTheWardore> lsSlots;
    public L10_WardoreDoorClose doorClose;
    

    public void Init()
    {
        gameObject.SetActive(false);
        Item_10_InTheWardore.postEvent += HandleDoneItem;
    }

    private void OnDestroy()
    {
        Item_10_InTheWardore.postEvent -= HandleDoneItem;
    }

    private void OnMouseDown()
    {
        gameObject.SetActive(false);
        doorClose.gameObject.SetActive(true);
        SetOpenedItems(false);
    }

    public void SetOpenedItems(bool state)
    {
        foreach (var item in lsItems)
        {
            if(item.GetItemPlaced()) continue;
            item.SetIsDoorOpened(state);
        }

        foreach (var slot in lsSlots)
        {
            if(slot.isFullSlot) continue;
            slot.SetIsDoorOpened(state);
        }
    }
    
    private void HandleDoneItem()
    {
        foreach(var item in lsItems)
            if(!item.GetItemPlaced()) return;
        
        gameObject.SetActive(false);
        doorClose.HandleDoneState();
    }
    
}