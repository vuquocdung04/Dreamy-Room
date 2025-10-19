using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using EventDispatcher;
using Sirenix.OdinInspector;
using UnityEngine;


public abstract class LevelBase : MonoBehaviour
{
    [SerializeField] protected bool isBoxReadyForInteraction;
    [SerializeField] protected int maxItemOutOfBox = 10;
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

    [Header("Combo")] [SerializeField] protected float comboThreshold = 2f;
    private float lastPlacementTime;

    // Cache trạng thái
    private bool lastHasItemOutOfBox;
    private bool lastHasReadyShadows;

    private EffectBoosterController effectBoosterController;
    private GamePlayController gamePlayController;
    private GameController gameController;

    public virtual void Init()
    {
        lastPlacementTime = -comboThreshold;
        gameController = GameController.Instance;
        gamePlayController = GamePlayController.Instance;
        if (gamePlayController == null)
        {
            Debug.Log("GamePlayController is null");
            return;
        }

        effectBoosterController = gamePlayController.effectBoosterController;
        foreach (var item in allItems)
        {
            item.Init(box.GetSpawnPos());
        }

        this.RegisterListener(EventID.REQUEST_TAKE_ITEM_FROM_BOX, TakeItemOutOfBox);
        this.RegisterListener(EventID.ITEM_PLACED_CORRECTLY, OnItemPlacedCorrectly);
        this.RegisterListener(EventID.SPAWN_STAR, SpawnStarEffect);
    }
    private void OnDestroy()
    {
        this.RemoveListener(EventID.REQUEST_TAKE_ITEM_FROM_BOX, TakeItemOutOfBox);
        this.RemoveListener(EventID.ITEM_PLACED_CORRECTLY, OnItemPlacedCorrectly);
        this.RemoveListener(EventID.SPAWN_STAR, SpawnStarEffect);
    }
    public void InitStateBox()
    {
        if(!isBoxReadyForInteraction) return;
        box.gameObject.SetActive(true);
        box.Init();
    }
    
    public void HideBox()
    {
        box.gameObject.SetActive(false);
    }

    public void SetActiveBox()
    {
        isBoxReadyForInteraction = true;
        InitStateBox();
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

        return allShadows.Exists(shadow => shadow.IsAvailableForMagicWand());
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
        item.ValidateUnlockState();
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
        effectBoosterController.EffectBooster(delegate { gamePlayController.gameScene.ActivateFrozeBooster(); });
    }

    public void UseHintBooster()
    {
        effectBoosterController.EffectBooster(delegate
        {
            foreach (var item in itemsOutOfBox)
            foreach (var slot in item.GetTargetSlot())
                if (slot != null && !slot.isFullSlot && slot.isReadyShow)
                {
                    item.SetPlacedByPlayer(false);
                    item.OnDoneSnap(slot);
                    return;
                }
        });
    }

    public void UseMagicWandBooster()
    {
        effectBoosterController.EffectBooster(delegate
        {
            ValidateInactiveShadows();

            List<ItemSlot> availableSlots = allShadows.Where(shadow =>
                shadow.isReadyShow && !shadow.isFullSlot && !shadow.gameObject.activeSelf).ToList();

            int countToTake = Mathf.Min(3, availableSlots.Count);
            List<ItemSlot> shadowsToShow = availableSlots.Take(countToTake).ToList();

            if (shadowsToShow.Count == 0) return;

            foreach (var shadow in shadowsToShow)
            {
                shadow.ActiveObj();
                shadow.SetActive();
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
            if (placedItem.GetPlacedByPlayer())
                SpawnCongratulationsEffect(placedItem);
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

    private void SpawnCongratulationsEffect(ItemBase placedItem)
    {
        float currentTime = Time.time;
        float timeDelta = currentTime - lastPlacementTime;
        if (timeDelta < comboThreshold)
            gameController.effectController.CongratulationEffect(placedItem.transform.position);
        lastPlacementTime = currentTime;
    }

    private void SpawnStarEffect(object obj = null)
    {
        if (!UseProfile.HasCompletedLevelTutorial) return;
        if (gamePlayController.IsWin) return;
        if (obj is not ItemBase item) return;
        var targetPos = gamePlayController.gameScene.GetStarBar();
        var spawnPos = item.transform.position;

        if (itemsPlacedCorrectly % 3 == 0)
        {
            gameController.effectController.StarEffect(spawnPos, targetPos.position,
                delegate { gamePlayController.gameScene.IncreaseStarAmount(); });
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
        gamePlayController.gameScene.SetFillProgressGame(itemsPlacedCorrectly, totalItemsRequired);
    }

    private void CheckWin()
    {
        if (itemsPlacedCorrectly == totalItemsRequired)
        {
            Debug.Log("WinGame");
            gameController.IncreaseLevel();
            ExecuteWinSequence().Forget();
        }
    }

    private async UniTask ExecuteWinSequence()
    {
        await PreWinGameLogic();

        await HandleAfterWinGame();
    }

    protected virtual async UniTask PreWinGameLogic()
    {
        transform.position = Vector3.zero;
        gamePlayController.WinGame();
        var duration = 0.75f;
        await gamePlayController.playerContains.mainCamera.DOOrthoSize(13f, duration).SetEase(Ease.Linear);
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        //NOTE: Viet o day
    }

    protected virtual async UniTask HandleAfterWinGame()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        WinBox.Setup().Show();
    }

    [Button("Setup Item", ButtonSizes.Large)]
    private void SetupItem()
    {
        items = transform.Find("Items");
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

        if (slots.childCount > 0) return;

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

            if (!shadowGo.TryGetComponent(out SpriteRenderer newSpr))
                newSpr = shadowGo.AddComponent<SpriteRenderer>();

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
            
            int targetOrder = item.GetIndexLayer() - 1;
            shadowSlot.SetupOdin(targetOrder, newSpr, item.GetSprite());
            shadowSlot.transform.SetParent(slots);
        }

        ItemSlot[] slotComponents = slots.GetComponentsInChildren<ItemSlot>(true);
        allShadows.AddRange(slotComponents);
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


    [Button("Add Conditions, Snap, Check Conditions Item && Slot", ButtonSizes.Large)]
    private void CheckStateReadyItemAndSlot()
    {
        if (allItems.Count == 0 || allShadows.Count == 0)
        {
            Debug.LogWarning("Cannot setup: Items or Shadows list is empty!");
            return;
        }

        for (int i = 0; i < allShadows.Count; i++)
        {
            allShadows[i].transform.localScale = allItems[i].transform.localScale;
        }
        inactiveShadows.Clear();
        for (int i = 0; i < allItems.Count; i++)
        {
            allItems[i].AddSnapSlot(allShadows[i]);
            allItems[i].AddConditionSlot(allShadows[i].conditionSlots);
            allItems[i].gameObject.layer = LayerMask.NameToLayer(LayerMaskName.ITEM_UNPLACED);
            allItems[i].SetStateItem();
        }

        foreach (var shadow in allShadows)
        {
            shadow.Init();
            if (!shadow.isReadyShow) inactiveShadows.Add(shadow);
            shadow.DeActiveObj();
        }
    }
}