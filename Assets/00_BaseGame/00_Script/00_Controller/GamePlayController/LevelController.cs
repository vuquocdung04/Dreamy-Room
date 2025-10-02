using UnityEngine;

public class LevelController : MonoBehaviour
{
    public LevelBase currentLevel;


    public void Init()
    {
        currentLevel.Init();
    }
}