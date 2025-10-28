using UnityEngine;

public class Level_11 : LevelBase
{
    protected override ItemSlot CreateItemSlotInstance(GameObject go)
    {
        return go.AddComponent<Slot_11>();
    }
}