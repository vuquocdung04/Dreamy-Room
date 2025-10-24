
using UnityEngine;

public class Item_10_Wardore : ItemBase, IPostPlacementAction
{
    public L10_WardoreDoorOpen doorOpenLeft;
    public L10_WardoreDoorOpen doorOpenRight;
    public L10_WardoreDoorClose doorCloseLeft;
    public L10_WardoreDoorClose doorCloseRight;

    public Transform hanger1;
    public Transform hanger2;
    public void HandlePostPlacementAction()
    {
        doorOpenLeft.Init();
        doorOpenRight.Init();
        doorCloseLeft.Init();
        doorCloseRight.Init();
        doorCloseLeft.EnableColl();
        doorCloseRight.EnableColl();
        hanger1.gameObject.SetActive(true);
        hanger2.gameObject.SetActive(true);
    }
}