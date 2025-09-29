using UnityEngine;

public class PlayerContains : MonoBehaviour
{
    public Camera mainCamera;
    public CameraController cameraController;
    public InputManager inputManager;
    public BoosterController boosterController;

    public void Init()
    {
        cameraController.Init();
        inputManager.Init();
        boosterController.Init();
        
    }
}