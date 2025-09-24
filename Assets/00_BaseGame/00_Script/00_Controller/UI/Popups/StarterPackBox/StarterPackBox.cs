using UnityEngine;
using UnityEngine.UI;

public class StarterPackBox : BoxSingleton<StarterPackBox>
{
    public static StarterPackBox Setup()
    {
        return Path(PathPrefabs.STARTER_PACK_BOX);
    }

    public Button btnClose;
    public Button btnPurchase;
    
    protected override void Init()
    {
        btnClose.onClick.AddListener(Close);
        
    }
    
    protected override void InitState()
    {
    }
}