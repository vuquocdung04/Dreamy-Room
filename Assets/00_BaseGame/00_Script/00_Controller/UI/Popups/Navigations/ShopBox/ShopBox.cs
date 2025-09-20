using UnityEngine;

public class ShopBox : BoxSingleton<ShopBox>
{
    public static ShopBox Setup()
    {
        return Setup(PathPrefabs.SHOP_BOX);
    }
    protected override void Init()
    {
        
    }

    protected override void InitState()
    {
        
    }
}