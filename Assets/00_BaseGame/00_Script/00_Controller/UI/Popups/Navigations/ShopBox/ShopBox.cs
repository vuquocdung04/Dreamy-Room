using UnityEngine;

public class ShopBox : BoxSingleton<ShopBox>
{
    public Canvas canvas;
    public static ShopBox Setup()
    {
        return Path(PathPrefabs.SHOP_BOX);
    }
    protected override void Init()
    {
        canvas.worldCamera = Camera.main;
    }

    protected override void InitState()
    {
        
    }
}