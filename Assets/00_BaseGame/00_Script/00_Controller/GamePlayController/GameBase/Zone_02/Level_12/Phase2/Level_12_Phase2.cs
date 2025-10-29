using UnityEngine;

public class Level_12_Phase2 : LevelBase
{
    public int GetTotalItemsRequired() => allItems.Count;
    protected override ItemSlot CreateItemSlotInstance(GameObject go)
    {
        return go.AddComponent<Slot_12>();
    }

    public override void Init()
    {
        base.Init();
        var level12 = (Level_12)GamePlayController.levelController.currentLevel;
        itemsPlacedCorrectly = level12.GetTotalItemRequired();
        totalItemsRequired = level12.totalProgressAllPhase;
        SetActiveBox();
        SetColorBox(Color.yellow);
    }
}