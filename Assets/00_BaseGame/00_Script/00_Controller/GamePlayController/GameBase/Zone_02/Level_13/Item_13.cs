using UnityEngine;

public class Item_13 : ItemBase, IPostPlacementAction
{
    public Transform lighting;
    
    public void HandlePostPlacementAction()
    {
        if(lighting)
            lighting.gameObject.SetActive(true);
    }
}