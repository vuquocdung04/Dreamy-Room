using UnityEngine;
using UnityEngine.UI;

public class TreasureBox :BoxSingleton<TreasureBox>
{
    public static TreasureBox Setup()
    {
        return Path(PathPrefabs.TREASURE_BOX);
    }

    public Button btnClose;
    public Image fillProgress;
    
    protected override void Init()
    {
        btnClose.onClick.AddListener(delegate
        {
            Debug.Log("Teeraera");
            Close();
        });
    }

    protected override void InitState()
    {
    }
}