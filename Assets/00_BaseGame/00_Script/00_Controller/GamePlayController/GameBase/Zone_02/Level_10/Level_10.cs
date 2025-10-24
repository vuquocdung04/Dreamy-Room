using UnityEngine;

public class Level_10 : LevelBase
{
    protected override ItemSlot CreateItemSlotInstance(GameObject go)
    {
        return go.AddComponent<Slot_10>();
    }
}