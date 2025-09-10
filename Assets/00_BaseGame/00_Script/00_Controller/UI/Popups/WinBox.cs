
using UnityEngine;
using UnityEngine.UI;

public class WinBox : BaseBox
{
    private static WinBox instance;
    public static WinBox SetUp()
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<WinBox>(PathPrefabs.WIN_BOX));
            instance.Init();
        }
        instance.InitState();
        return instance;
        
    }

    public Button btnClose;
    private void Init()
    {
        // Khoi tao 1 lan
        btnClose.onClick.AddListener(Close);
    }

    private void InitState()
    {
        // Pooling sau lan thu nhat
    }
}
