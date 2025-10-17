
using System.Collections.Generic;
using UnityEngine;

public class Item_4_FridgeDoor : MonoBehaviour
{
    public Collider2D coll;
    private bool isShow;
    public Transform doorOpen;
    public Transform doorClose;
    public List<Slot_4_InTheFridge> lsItemSlots;
    public List<Item_4_InTheFridge> lsItems;
    private void Start()
    {
        Item_4_InTheFridge.postEvent += AreItemsDone;
    }

    private void OnMouseDown()
    {
        if (isShow) return;
        isShow = true;
        coll.enabled = false;
        foreach(var slot in lsItemSlots) slot.SetActiveWhenOpened();
        foreach(var slot in lsItems) slot.SetDoorOpened();
        foreach(var slot in lsItems) slot.ValidateUnlockState();
        doorOpen.gameObject.SetActive(true);
        doorClose.gameObject.SetActive(false);
    }

    private void AreItemsDone()
    {
        foreach (var item in lsItems)
        {
            if(!item.GetItemPlaced()) return;
        }
        foreach(var item in lsItems)
            item.transform.SetParent(doorOpen);
        foreach(var slot in lsItemSlots)
            slot.transform.SetParent(doorOpen);
        
        doorOpen.gameObject.SetActive(false);
        doorClose.gameObject.SetActive(true);
    }
    private void OnDestroy()
    {
        Item_4_InTheFridge.postEvent -= AreItemsDone;
    }

}