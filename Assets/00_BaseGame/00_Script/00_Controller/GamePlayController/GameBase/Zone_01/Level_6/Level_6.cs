using UnityEngine;

public class Level_6 : LevelBase
{
    protected override ItemSlot CreateItemSlotInstance(GameObject go)
    {
        return go.AddComponent<Slot_6>();
    }
    
}