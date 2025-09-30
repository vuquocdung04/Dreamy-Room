
using UnityEngine;

public class PlayerContains : MonoBehaviour
{
    
    public Camera mainCamera;
    public CameraController cameraController;
    public InputManager inputManager;
    public BoosterController boosterController;
    public Transform top;
    public Transform bottom;
    public Transform left;
    public Transform right;
    
    
    public void Init()
    {
        cameraController.Init();
        inputManager.Init();
        boosterController.Init();
    }

    private void AdjustCamera()
    {
        float baseHeight = mainCamera.orthographicSize * 2f;
        float baseWidth = baseHeight * mainCamera.aspect;
        float targetAspectRatio = baseWidth / baseHeight;
        float windowAspectRatio = (float)Screen.width / Screen.height;
        
        float scaleHeight = windowAspectRatio / targetAspectRatio;
        if (scaleHeight < 1)
            mainCamera.orthographicSize = baseHeight * 0.5f /scaleHeight;
        else
            mainCamera.orthographicSize = baseHeight * 0.5f;
    }

    private void OnRectTransformDimensionsChange()
    {
        AdjustCamera();
        Debug.Log("Dieu chin cam");
    }
}