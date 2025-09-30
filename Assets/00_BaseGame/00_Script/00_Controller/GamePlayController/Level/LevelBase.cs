using System;
using System.Collections.Generic;
using System.Linq;
using EventDispatcher;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelBase : MonoBehaviour
{
    [SerializeField] private int maxItemOutOfBox = 10;
    [SerializeField] private float snapThreshold;
    [SerializeField] private List<ItemSlot> allShadows;
    [SerializeField] private List<ItemBase> allItems;
    [SerializeField] private List<ItemBase> itemsOutOfBox;

    [Header("Box Setting")]
    [SerializeField] private Transform boxPosition;

    [SerializeField] private Vector2 spawnOffset;
    
    public void Start()
    {
        this.RegisterListener(EventID.UPDATE_UNLOCK_ITEM, UpdateStateUnlockItem);
        this.RegisterListener(EventID.TAKE_OUT_ITEM, TakeItemOutOfBox);
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.UPDATE_UNLOCK_ITEM, UpdateStateUnlockItem);
        this.RemoveListener(EventID.TAKE_OUT_ITEM, TakeItemOutOfBox);
    }

    private void TakeItemOutOfBox(object obj = null)
    {
        if (itemsOutOfBox.Count >= maxItemOutOfBox)
        {
            Debug.Log("items out of box full");
            return;
        }
        
        if(itemsOutOfBox == null && itemsOutOfBox.Count == 0) return;
        ItemBase item = allItems[0];
        allItems.RemoveAt(0);
        AddItemToOutOfBox(item);

        Vector2 spawnPos = boxPosition.position;
        item.OutSideBox(spawnPos);
        
    }
    private void UpdateStateUnlockItem(object obj = null)
    {
        foreach (var item in itemsOutOfBox)
        {
            item.ValidateUnlockState();
        }
    }
    
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


    [Button("Setup Item", ButtonSizes.Large)]
    public void SetupItem()
    {
        boxPosition = transform.Find("Box").GetComponent<Transform>();
        foreach (var item in this.allItems)
        {
            item.SetupOdin();
        }
    }
}