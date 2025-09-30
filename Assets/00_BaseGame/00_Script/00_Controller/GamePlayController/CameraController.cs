using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;
    private float defaultOrthographicSize;
    private float zoomedOrthographicSize;
    private float targetSize;
    private bool isZoomed;
    
    private Vector3 cameraPosition;
    [SerializeField] private float limit = 6f;

    public void Init()
    {
        mainCamera = GamePlayController.Instance.playerContains.mainCamera;
        cameraPosition = mainCamera.transform.position;
        defaultOrthographicSize = mainCamera.orthographicSize;
        zoomedOrthographicSize = defaultOrthographicSize - 2;

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
        
        float deltaScreenX = currentScreenPosition.x - lastScreenPosition.x;
        
        float deltaWorldX = mainCamera.ScreenToWorldPoint(new Vector3(deltaScreenX, 0, 0)).x 
                            - mainCamera.ScreenToWorldPoint(Vector3.zero).x;
        
        cameraPosition.x = Mathf.Clamp(cameraPosition.x - deltaWorldX, -limit, limit);
        mainCamera.transform.position = cameraPosition;
        
        lastScreenPosition.x = currentScreenPosition.x;
        lastScreenPosition.y = currentScreenPosition.y;
    }
}