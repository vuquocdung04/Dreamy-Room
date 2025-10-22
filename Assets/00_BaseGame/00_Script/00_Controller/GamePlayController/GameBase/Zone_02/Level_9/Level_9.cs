using UnityEngine;

public class Level_9 : LevelBase
{
    protected override ItemSlot CreateItemSlotInstance(GameObject go)
    {
        return go.AddComponent<Slot_9>();
    }
}