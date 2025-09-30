using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Camera mainCamera;
    private CameraController cameraController;
    
    private bool isDraggingCamera;
    private ItemBase currentDraggingItem; // Item đang được drag

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
                BoxGameBase box = hit.collider.GetComponent<BoxGameBase>();
                if (box != null)
                {
                    box.OnBoxClicked();
                    return;
                }
                
                // Kiểm tra xem có phải ItemBase không
                ItemBase item = hit.collider.GetComponent<ItemBase>();
                if (item != null)
                {
                    currentDraggingItem = item;
                    currentDraggingItem.OnStartDrag();
                }
                else
                {
                    // Nếu click vào object khác (không phải item) thì vẫn drag camera
                    isDraggingCamera = true;
                    lastScreenPosition = Input.mousePosition;
                }
            }

            prevMousePosition = currentMousePosition;
        }

        // Xử lý drag
        if (isDraggingCamera)
        {
            cameraController.MoveCamera(ref lastScreenPosition);
        }
        else if (currentDraggingItem != null)
        {
            Vector3 delta = currentMousePosition - prevMousePosition;
            currentDraggingItem.OnDrag(delta);
        }
        
        prevMousePosition = currentMousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            if (currentDraggingItem != null)
            {
                float snapThreshold = 1f;
                currentDraggingItem.OnEndDrag(snapThreshold);
                currentDraggingItem = null;
            }
            
            isDraggingCamera = false;
        }
    }
}