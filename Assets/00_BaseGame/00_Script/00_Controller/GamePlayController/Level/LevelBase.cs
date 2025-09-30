using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelBase : MonoBehaviour
{
    
    [SerializeField] private float snapThreshold;
    [SerializeField] private List<ItemSlot> allShadows;
    [SerializeField] private List<ItemBase> allItems;
    [SerializeField] private List<ItemBase> itemsOutOfBox;


    public void UseHintBooster()
    {
        foreach (var item in itemsOutOfBox)
        {
            foreach (var slot in item.GetTargetSlot())
            {
                if (slot != null && !slot.isFullSlot && slot.isReadyShow)
                {
                    item.OnDoneSnap(slot);
                    return;
                }
            }
        }
    }
    
    public void UseMagicWandBooster()
    {
        foreach (var shadow in allShadows)
        {
            if (!shadow.isFullSlot)
            {
                shadow.ValidateReadyState();
            }
        }
        
        List<ItemSlot> availableSlots = allShadows.Where(shadow => 
            shadow.isReadyShow && !shadow.gameObject.activeSelf).ToList();
    
        int countToTake = Mathf.Min(3, availableSlots.Count);
        List<ItemSlot> shadowsToShow = availableSlots.Take(countToTake).ToList();
    
        if(shadowsToShow.Count == 0) return;
    
        foreach (var shadow in shadowsToShow)
        {
            shadow.Active();
            allShadows.Remove(shadow);
        }
    }

    public void AddItemToOutOfBox(ItemBase item)
    {
        if(!itemsOutOfBox.Contains(item))
        {
            itemsOutOfBox.Add(item);
        }
    }
    public void OnItemPlacedCorrectly(ItemBase placedItem)
    {
        if (itemsOutOfBox.Contains(placedItem))
        {
            itemsOutOfBox.Remove(placedItem);
        }
    }
}