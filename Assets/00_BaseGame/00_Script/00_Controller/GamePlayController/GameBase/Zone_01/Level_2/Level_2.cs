
using UnityEngine;

public class Level_2 : LevelBase
{
    protected override ItemSlot CreateItemSlotInstance(GameObject go)
    {
        return go.AddComponent<Slot_2>();
    }
}