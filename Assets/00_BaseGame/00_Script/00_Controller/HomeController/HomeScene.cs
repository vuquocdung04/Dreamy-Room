
using UnityEngine;
using UnityEngine.UI;

public class HomeScene : MonoBehaviour
{
    public ConsumableBox consumableBox;
    public NavController navController;
    public void Init()
    {
        consumableBox.Init();
        navController.Init();
    }
}
