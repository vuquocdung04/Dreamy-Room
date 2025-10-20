using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Level_5 : LevelBase
{
    public List<PrevItem_5> lsPrevItems;
    public int curProgressPrev;
    public ItemRemover_5_Broom broom;
    public Transform targetPlacedPrevItem;
    private int maxLevel;
    private int currentLevel;
    public override void Init()
    {
        base.Init();
        broom.Init();
        isBoxReadyForInteraction = false;
        broom.gameObject.SetActive(false);
        foreach (var prevItem in lsPrevItems)
        {
            prevItem.Init();
        }

        maxLevel = UseProfile.MaxUnlockedLevel;
        currentLevel = UseProfile.CurrentLevel;
    }

    private void Update()
    {
        if(maxLevel != 5 && currentLevel == 5) return;
        if (Input.GetMouseButtonDown(0))
        {
            GamePlayController.Instance.gameScene.HideSwipeCam();
        }
    }

    protected override ItemSlot CreateItemSlotInstance(GameObject go)
    {
        return go.AddComponent<Slot_5>();
    }
    
    public void HandlePrevItem()
    {
        curProgressPrev++;
        if (curProgressPrev >= lsPrevItems.Count)
        {
            broom.transform.localScale = Vector3.zero;
            broom.gameObject.SetActive(true);
            broom.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        }
    }

    public void CleanDirtCompleted()
    {
        broom.gameObject.SetActive(true);
        SetActiveBox();
    }
    
}