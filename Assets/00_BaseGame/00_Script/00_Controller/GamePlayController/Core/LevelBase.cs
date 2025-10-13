using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using EventDispatcher;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class LevelBase : MonoBehaviour
{
    [SerializeField] protected bool isBoxReadyForInteraction;
    [SerializeField] protected int maxItemOutOfBox = 10;
    [SerializeField] protected float snapThreshold;
    [SerializeField] protected List<ItemSlot> allShadows;
    [SerializeField] protected List<ItemBase> allItems;

    [Header("Debug"), Space(5)] [SerializeField]
    protected List<ItemSlot> inactiveShadows = new();

    [SerializeField] protected List<ItemBase> itemsOutOfBox = new();

    [Header("Box Setting")] [SerializeField]
    protected Transform slots;

    [SerializeField] protected Transform items;
    [SerializeField] private BoxGameBase box;

    [Header("Game Setting")] [SerializeField]
    protected int totalItemsRequired;

    [SerializeField] protected int itemsPlacedCorrectly;

    // Cache trạng thái
    private bool lastHasItemOutOfBox;
    private bool lastHasReadyShadows;

    private EffectController effectController;

    public virtual void Init()
    {
        this.RegisterListener(EventID.REQUEST_TAKE_ITEM_FROM_BOX, TakeItemOutOfBox);
        this.RegisterListener(EventID.ITEM_PLACED_CORRECTLY, OnItemPlacedCorrectly);
        effectController = GamePlayController.Instance.effectController;
        foreach (var item in allItems)
        {
            item.Init(box.GetSpawnPos());
        }

    }
    

    private void OnDestroy()
    {
        this.RemoveListener(EventID.REQUEST_TAKE_ITEM_FROM_BOX, TakeItemOutOfBox);
        this.RemoveListener(EventID.ITEM_PLACED_CORRECTLY, OnItemPlacedCorrectly);
    }


    public void SetColorBox(Color color)
    {
        box.SetColor(color);
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

        if (allItems == null || allItems.Count == 0) return;
        ItemBase item = allItems[0];
        item.gameObject.SetActive(true);
        allItems.RemoveAt(0);
        AddItemToOutOfBox(item);
        if (box.HasBoxOpened())
            box.PlayAnimation();
        if (itemsOutOfBox.Count >= maxItemOutOfBox) box.CloseBox();

        Vector2 spawnPos = box.GetSpawnPos().position;
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
        effectController.EffectBooster(delegate { GamePlayController.Instance.gameScene.ActivateFrozeBooster(); });
    }

    public void UseHintBooster()
    {
        effectController.EffectBooster(delegate
        {
            foreach (var item in itemsOutOfBox)
            foreach (var slot in item.GetTargetSlot())
                if (slot != null && !slot.isFullSlot && slot.isReadyShow)
                {
                    item.OnDoneSnap(slot);
                    return;
                }
        });
    }

    public void UseMagicWandBooster()
    {
        effectController.EffectBooster(delegate
        {
            ValidateInactiveShadows();

            List<ItemSlot> availableSlots = allShadows.Where(shadow =>
                shadow.isReadyShow && !shadow.isFullSlot && !shadow.gameObject.activeSelf).ToList();

            int countToTake = Mathf.Min(3, availableSlots.Count);
            List<ItemSlot> shadowsToShow = availableSlots.Take(countToTake).ToList();

            if (shadowsToShow.Count == 0) return;

            foreach (var shadow in shadowsToShow)
            {
                shadow.Active();
                shadow.transform.DOJump(shadow.transform.position, 2f, 1, 0.1f);
            }

            this.PostEvent(EventID.ON_BOOSTER_CONDITION_CHANGED);
        });
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
                HandleFillProgress();
                CheckWin();

                CheckAndReopenBox();

                CheckAndPostBoosterConditionChanged();
            }

            foreach (var item in itemsOutOfBox)
                item.ValidateUnlockState();
        }
    }

    private void CheckAndReopenBox()
    {
        if (itemsOutOfBox.Count < maxItemOutOfBox)
        {
            box.ReopenBox();
        }
    }

    private void HandleFillProgress()
    {
        GamePlayController.Instance.gameScene.SetFillProgressGame(itemsPlacedCorrectly, totalItemsRequired);
    }

    private void CheckWin()
    {
        if (itemsPlacedCorrectly == totalItemsRequired)
        {
            Debug.Log("WinGame");
            HandleAfterWinGame();
        }
    }

    protected virtual void HandleAfterWinGame()
    {
        GamePlayController.Instance.WinGame();
        GamePlayController.Instance.playerContains.mainCamera.DOOrthoSize(14f, 0.75f).SetEase(Ease.Linear).OnComplete(
            delegate
            {
                WinBox.Setup().Show();
            });
    }

    [Button("Setup Item", ButtonSizes.Large)]
    private void SetupItem()
    {
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
        ItemBase[] itemComponents = items.GetComponentsInChildren<ItemBase>(true);
        allItems.AddRange(itemComponents);
        totalItemsRequired = allItems.Count;
        foreach (var item in this.allItems)
        {
            item.SetupOdin();
        }
        Debug.Log("Finish Setup Item");
    }

    [Button("Create && Setup Shadow Item", ButtonSizes.Large)]
    private void CreateShadowItem()
    {
        slots = transform.Find("Slots");
        allShadows.Clear();
        
        if(slots.childCount > 0) return;
        
        if (allItems.Count == 0)
        {
            Debug.Log("No Items Count, can't create Shadow Item");
            return;
        }

        foreach (var item in allItems)
        {
            var shadowGo = new GameObject(item.name + "_shadow");
            shadowGo.transform.SetParent(slots);
            shadowGo.transform.position = item.transform.position;
            
            if (!shadowGo.TryGetComponent(out SpriteRenderer sr))
                sr = shadowGo.AddComponent<SpriteRenderer>();
        
            var shadowSlot = shadowGo.GetComponent<ItemSlot>();
            if (shadowSlot == null)
            {
                shadowSlot = CreateItemSlotInstance(shadowGo);
                if (shadowSlot == null)
                {
                    Debug.LogError($"Failed to create ItemSlot for {shadowGo.name}");
                    Destroy(shadowGo);
                    continue;
                }
            }
            item.AddSnapSlot(shadowSlot);
            int targetOrder = item.GetIndexLayer() - 1;
            shadowSlot.SetupOdin(targetOrder,sr);
            shadowSlot.transform.SetParent(slots);
        }
        ItemSlot[] slotComponents = slots.GetComponentsInChildren<ItemSlot>(true);
        allShadows.AddRange(slotComponents);
        foreach (var shadow in allShadows)
        {
            shadow.Init();
            if (!shadow.isReadyShow) inactiveShadows.Add(shadow);
            shadow.DeActive();
        }
        Debug.Log("Finish Setup Shadow Item");
    }

    protected virtual ItemSlot CreateItemSlotInstance(GameObject go)
    {
        if (typeof(ItemSlot).IsAbstract)
        {
            Debug.LogError("ItemSlot is abstract! Override CreateItemSlotInstance in subclass.");
            return null;
        }
        return go.AddComponent<ItemSlot>();
    }
}