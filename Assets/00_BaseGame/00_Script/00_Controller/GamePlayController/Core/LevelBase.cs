using System.Collections.Generic;
using System.Linq;
using EventDispatcher;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelBase : MonoBehaviour
{
    [SerializeField] private bool isBoxReadyForInteraction;
    [SerializeField] private int maxItemOutOfBox = 10;
    [SerializeField] private float snapThreshold;
    [SerializeField] private List<ItemSlot> allShadows;
    [SerializeField] private List<ItemBase> allItems;
    [Header("Debug"),Space(5)]
    [SerializeField] private List<ItemSlot> inactiveShadows = new();
    [SerializeField] private List<ItemBase> itemsOutOfBox = new();

    [Header("Box Setting")] [SerializeField]
    private Transform slots;

    [SerializeField] private Transform items;
    [SerializeField] private BoxGameBase box;

    [Header("Game Setting")] [SerializeField]
    private int totalItemsRequired;

    [SerializeField] private int itemsPlacedCorrectly;

    // Cache trạng thái
    private bool lastHasItemOutOfBox;
    private bool lastHasReadyShadows;

    public virtual void Init()
    {
        this.RegisterListener(EventID.REQUEST_TAKE_ITEM_FROM_BOX, TakeItemOutOfBox);
        this.RegisterListener(EventID.ITEM_PLACED_CORRECTLY, OnItemPlacedCorrectly);

        foreach (var shadow in allShadows)
        {
            shadow.Init();
            if(!shadow.isReadyShow) inactiveShadows.Add(shadow);
            shadow.DeActive();
        }
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.REQUEST_TAKE_ITEM_FROM_BOX, TakeItemOutOfBox);
        this.RemoveListener(EventID.ITEM_PLACED_CORRECTLY, OnItemPlacedCorrectly);
    }

    public void SetBoxReadyForInteraction(bool ready)
    {
        isBoxReadyForInteraction = ready;
        CheckAndPostBoosterConditionChanged();
    }
    
    public bool HasItemOutOfBox() => isBoxReadyForInteraction && itemsOutOfBox.Count > 0;

    public bool HasReadyShadowsForMagicWand()
    {
        if (!isBoxReadyForInteraction) return false;
        
        return allShadows.Exists(shadow => 
            shadow.isReadyShow && 
            !shadow.isFullSlot && 
            !shadow.gameObject.activeSelf);
    }

    private void TakeItemOutOfBox(object obj = null)
    {
        if (itemsOutOfBox.Count >= maxItemOutOfBox)
        {
            Debug.Log("items out of box full");
            return;
        }

        if (itemsOutOfBox == null && itemsOutOfBox.Count == 0) return;
        ItemBase item = allItems[0];
        allItems.RemoveAt(0);
        AddItemToOutOfBox(item);
        Vector2 spawnPos = box.transform.position;
        item.OutSideBox(spawnPos);
        if (allItems.Count == 0)
            box.ScaleToZero();
        CheckAndPostBoosterConditionChanged();
    }

    private void CheckAndPostBoosterConditionChanged()
    {
        bool currentHasItemOutOfBox = HasItemOutOfBox();
        bool currentHasReadyShadows = HasReadyShadowsForMagicWand();

        if (currentHasItemOutOfBox != lastHasItemOutOfBox ||
            currentHasReadyShadows != lastHasReadyShadows)
        {
            lastHasItemOutOfBox = currentHasItemOutOfBox;
            lastHasReadyShadows = currentHasReadyShadows;
            this.PostEvent(EventID.ON_BOOSTER_CONDITION_CHANGED);
        }
    }

    public void UseFrozeBooster()
    {
        var gamePlayController = GamePlayController.Instance;
        gamePlayController.effectController.EffectBooster(delegate
        {
            gamePlayController.gameScene.ActivateFrozeBooster();
        });
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
        ValidateInactiveShadows();
        
        List<ItemSlot> availableSlots = allShadows.Where(shadow =>
            shadow.isReadyShow && !shadow.isFullSlot &&!shadow.gameObject.activeSelf).ToList();

        int countToTake = Mathf.Min(3, availableSlots.Count);
        List<ItemSlot> shadowsToShow = availableSlots.Take(countToTake).ToList();

        if (shadowsToShow.Count == 0) return;

        foreach (var shadow in shadowsToShow)
            shadow.Active();
        this.PostEvent(EventID.ON_BOOSTER_CONDITION_CHANGED);
    }
    private void ValidateInactiveShadows()
    {
        for (int i = inactiveShadows.Count - 1; i >= 0; i--)
        {
            var shadow = inactiveShadows[i];
            shadow.ValidateReadyState();
            if (shadow.isReadyShow)
                inactiveShadows.RemoveAt(i);
        }
    }
    private void AddItemToOutOfBox(ItemBase item)
    {
        if (!itemsOutOfBox.Contains(item))
            itemsOutOfBox.Add(item);
    }

    private void OnItemPlacedCorrectly(object obj = null)
    {
        if (obj is ItemBase placedItem)
        {
            if (itemsOutOfBox.Contains(placedItem))
            {
                itemsOutOfBox.Remove(placedItem);
                itemsPlacedCorrectly++;
                
                ValidateInactiveShadows();
                
                CheckWin();
                CheckAndPostBoosterConditionChanged();
            }

            foreach (var item in itemsOutOfBox)
                item.ValidateUnlockState();
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