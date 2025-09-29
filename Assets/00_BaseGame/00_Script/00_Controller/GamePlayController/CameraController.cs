using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private  Camera mainCamera;
    private readonly float defaultOrthographicSize = 10f;
    private readonly float zoomedOrthographicSize = 8f;
    private float targetSize;
    private bool isZoomed;

    public void Init()
    {
        mainCamera = GamePlayController.Instance.playerContains.mainCamera;
    }
    
    public bool GetIsZoomed() => isZoomed;

    public void ToggleZoomInOut()
    {
        targetSize = isZoomed ? defaultOrthographicSize : zoomedOrthographicSize;
        mainCamera.DOOrthoSize(targetSize, 0.2f);
        isZoomed = !isZoomed;
    }

    public void MoveCamera(ref Vector3 lastScreenPosition)
    {
        Vector3 currentScreenPosition = Input.mousePosition;
        Vector3 deltaScreen = currentScreenPosition - lastScreenPosition;
        
        // Chuyển đổi delta từ screen space sang world space
        Vector3 deltaWorld = mainCamera.ScreenToWorldPoint(deltaScreen) - mainCamera.ScreenToWorldPoint(Vector3.zero);
        
        mainCamera.transform.position -= new Vector3(deltaWorld.x, deltaWorld.y, 0);
        lastScreenPosition = currentScreenPosition;
    }
}