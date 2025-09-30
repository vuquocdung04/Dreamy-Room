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
    [SerializeField] private Transform slots;
    [SerializeField] private Transform items;
    [SerializeField] private BoxGameBase box;
    
    [Header("Game Setting")]
    [SerializeField] private int totalItemsRequired;
    [SerializeField] private int itemsPlacedCorrectly;
    public void Start()
    {
        this.RegisterListener(EventID.REQUEST_TAKE_ITEM_FROM_BOX, TakeItemOutOfBox);
        this.RegisterListener(EventID.ITEM_PLACED_CORRECTLY, OnItemPlacedCorrectly);
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.REQUEST_TAKE_ITEM_FROM_BOX, TakeItemOutOfBox);
        this.RemoveListener(EventID.ITEM_PLACED_CORRECTLY, OnItemPlacedCorrectly);
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
        Vector2 spawnPos = box.transform.position;
        item.OutSideBox(spawnPos);
        
        if (allItems.Count == 0)
        {
            box.ScaleToZero();
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

    private void AddItemToOutOfBox(ItemBase item)
    {
        if(!itemsOutOfBox.Contains(item))
        {
            itemsOutOfBox.Add(item);
        }
    }
    private void OnItemPlacedCorrectly(object obj = null)
    {
        if (obj is ItemBase placedItem)
        {
            if (itemsOutOfBox.Contains(placedItem))
            {
                itemsOutOfBox.Remove(placedItem);
                itemsPlacedCorrectly++;
                CheckWin();
            }
            foreach (var item in itemsOutOfBox)
            {
                item.ValidateUnlockState();
            }
        }
    }

    private void CheckWin()
    {
        if (itemsPlacedCorrectly == totalItemsRequired)
        {
            Debug.Log("WIn Game");
        }
    }


    [Button("Setup Item", ButtonSizes.Large)]
    public void SetupItem()
    {
        slots = transform.Find("Slots");
        items = transform.Find("Items");
        Transform boxTransform = transform.Find("Box");
        if (boxTransform != null)
        {
            box = boxTransform.GetComponent<BoxGameBase>();
            if (box == null)
            {
                box = boxTransform.gameObject.AddComponent<BoxGameBase>();
            }
        }
        allItems.Clear();
        allShadows.Clear();
        ItemSlot[] slotComponents = slots.GetComponentsInChildren<ItemSlot>(true); 
        allShadows.AddRange(slotComponents);
        
        ItemBase[] itemComponents = items.GetComponentsInChildren<ItemBase>(true); 
        allItems.AddRange(itemComponents);
        
        totalItemsRequired = allItems.Count;
        
        foreach (var item in this.allItems)
        {
            item.SetupOdin();
        }
    }
}