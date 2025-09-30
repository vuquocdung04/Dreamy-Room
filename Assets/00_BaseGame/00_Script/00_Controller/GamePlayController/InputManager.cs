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
    private Vector3 delta;
    
    private float left,top,right,bottom;
    public void Init()
    {
        var playerContains = GamePlayController.Instance.playerContains;
        mainCamera = playerContains.mainCamera;
        cameraController = playerContains.cameraController;

        left = playerContains.left.transform.localPosition.x;
        top = playerContains.top.transform.localPosition.y;
        right = playerContains.right.transform.localPosition.x;
        bottom = GameController.Instance.useProfile.IsRemoveAds ? playerContains.bottom.transform.localPosition.y :  playerContains.bottom.transform.localPosition.y + 2f;
        
        
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
                    currentDraggingItem.OnStartDrag(top,currentMousePosition);
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
            delta = currentMousePosition - prevMousePosition;
            currentDraggingItem.OnDrag(delta,left,right,bottom,top);
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