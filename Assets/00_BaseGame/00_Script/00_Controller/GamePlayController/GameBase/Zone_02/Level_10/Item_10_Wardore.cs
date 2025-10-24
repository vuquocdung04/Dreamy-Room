
using UnityEngine;

public class Item_10_Wardore : ItemBase, IPostPlacementAction
{
    public L10_WardoreDoorOpen doorOpenLeft;
    public L10_WardoreDoorOpen doorOpenRight;
    public L10_WardoreDoorClose doorCloseLeft;
    public L10_WardoreDoorClose doorCloseRight;

    public void HandlePostPlacementAction()
    {
        doorOpenLeft.Init();
        doorOpenRight.Init();
        doorCloseLeft.Init();
        doorCloseRight.Init();
        doorCloseLeft.EnableColl();
        doorCloseRight.EnableColl();
    }
}