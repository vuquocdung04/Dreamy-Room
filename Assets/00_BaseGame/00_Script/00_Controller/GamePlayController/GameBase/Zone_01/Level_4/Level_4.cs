using UnityEngine;

public class Level_4 : LevelBase
{
    protected override ItemSlot CreateItemSlotInstance(GameObject go)
    {
        return go.AddComponent<Slot_4>();
    }
}