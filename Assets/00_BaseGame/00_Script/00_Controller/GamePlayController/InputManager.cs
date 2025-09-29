using UnityEngine;

public class InputManager : MonoBehaviour
{

    private Camera mainCamera;
    private CameraController cameraController;
    
    private bool isDraggingCamera;

    // Cho camera dragging (dùng screen space)
    private Vector3 lastScreenPosition;

    // Cho object dragging (dùng world space)
    private Vector3 currentMousePosition;
    private Vector3 prevMousePosition;

    public void Init()
    {
        var playerContains = GamePlayController.Instance.playerContains;
        mainCamera = playerContains.mainCamera;
        cameraController = playerContains.cameraController;
    }
    
    private void Update()
    {
        if (cameraController == null)
        {
            Debug.Log("CameraController is null");
            return;
        }
        currentMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        currentMousePosition.z = 0;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(currentMousePosition, Vector2.zero);

            if (hit.collider == null)
            {
                isDraggingCamera = true;
                lastScreenPosition = Input.mousePosition;
            }
            else
            {
            }

            prevMousePosition = currentMousePosition;
        }

        if (isDraggingCamera)
        {
            cameraController.MoveCamera(ref lastScreenPosition);
            prevMousePosition = currentMousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDraggingCamera = false;
        }
    }
}