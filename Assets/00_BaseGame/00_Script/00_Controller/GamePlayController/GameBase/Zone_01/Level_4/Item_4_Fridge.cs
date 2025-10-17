
using UnityEngine;

public class Item_4_Fridge : Item_4, IPostPlacementAction
{
    [Header("Refrigerator Setting")]
    public Item_4_FridgeDoor doorTop;
    public Item_4_FridgeDoor doorBottom;
    
    public void HandlePostPlacementAction()
    {
        doorTop.coll.enabled = true;
        doorBottom.coll.enabled = true;
    }
}