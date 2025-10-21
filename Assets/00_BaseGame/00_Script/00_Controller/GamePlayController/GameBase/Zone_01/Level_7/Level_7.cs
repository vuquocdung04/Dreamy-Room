using System.Collections.Generic;
using UnityEngine;

public class Level_7 : LevelBase
{
    public Transform bin;
    public List<PrevItem_7> lsGarbage;
    public int curProgressPrev;
    
    protected override ItemSlot CreateItemSlotInstance(GameObject go)
    {
        return go.AddComponent<Slot_7>();
    }

    public override void Init()
    {
        base.Init();
        isBoxReadyForInteraction = false;
        foreach(var garbage in lsGarbage)
            garbage.Init();
    }

    public void HandlePrevItem()
    {
        curProgressPrev++;
        if (curProgressPrev == lsGarbage.Count)
        {
            SetActiveBox();
        }
    }
}