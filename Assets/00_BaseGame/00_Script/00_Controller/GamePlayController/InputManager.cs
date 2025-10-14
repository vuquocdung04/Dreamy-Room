using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Camera mainCamera;
    private CameraController cameraController;
    private PlayerContains playerContains;

    private bool isDraggingCamera;
    private ItemBase currentDraggingItem;
    private PreGameItem currentPreGameDraggingItem;
    private ItemRemoverBase currentDrawingItem; // Thêm drawing item

    private Vector3 lastScreenPosition;
    private Vector3 currentMousePosition;
    private Vector3 prevMousePosition;
    private Vector3 mouseDelta;
    private float left, top, right, bottom;

    [SerializeField] private bool isWin, isLose, isPopupOpen, canMoveCamera;
    
    [Header("Drawing Settings")]
    [SerializeField] private float drawApplyInterval = 0.05f;
    private float lastDrawApplyTime;
    [Header("Physics Layers")]
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private LayerMask boxLayer;
    [SerializeField] private LayerMask defaultLayer;
    
    public void Init()
    {
        playerContains = GamePlayController.Instance.playerContains;
        mainCamera = playerContains.mainCamera;
        cameraController = playerContains.cameraController;
        UpdateBounds();
    }

    private void Update()
    {
        if (isWin || isPopupOpen || isLose) return;

        currentMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        currentMousePosition.z = 0;

        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseDown();
            prevMousePosition = currentMousePosition;
        }

        // Xử lý drag
        HandleDragging();

        if (Input.GetMouseButtonUp(0))
        {
            HandleMouseUp();
        }
    }

    private void HandleMouseDown()
    {
        // Ưu tiên 1: Check Box và Item Layer
        LayerMask highPriorityLayers = boxLayer | itemLayer;
        RaycastHit2D highPriorityHit = Physics2D.Raycast(currentMousePosition, Vector2.zero, Mathf.Infinity, highPriorityLayers);
    
        if (highPriorityHit.collider != null)
        {
            if (TryHandleBox(highPriorityHit.collider)) return;
            if (TryHandleDrawingItem(highPriorityHit.collider)) return;
            if (TryHandleItem(highPriorityHit.collider)) return;
        }
        
        RaycastHit2D defaultHit = Physics2D.Raycast(currentMousePosition, Vector2.zero, Mathf.Infinity, defaultLayer);
        if (defaultHit.collider != null)
        {
            if (TryHandleItem(defaultHit.collider)) return;
            if (TryHandlePreGameItem(defaultHit.collider)) return;
        }
        StartCameraDrag();
    }

    private bool TryHandleBox(Collider2D coll)
    {
        BoxGameBase box = coll.GetComponent<BoxGameBase>();
        if (box != null)
        {
            box.OnBoxClicked();
            return true;
        }
        return false;
    }

    private bool TryHandleDrawingItem(Collider2D coll)
    {
        ItemRemoverBase drawingItem = coll.GetComponent<ItemRemoverBase>();
        if (drawingItem != null)
        {
            currentDrawingItem = drawingItem;
            lastDrawApplyTime = Time.time;
            return true;
        }
        return false;
    }

    private bool TryHandleItem(Collider2D coll)
    {
        ItemBase item = coll.GetComponent<ItemBase>();
        if (item != null)
        {
            currentDraggingItem = item;
            currentDraggingItem.OnStartDrag(top, currentMousePosition);
            return true;
        }
        return false;
    }

    private bool TryHandlePreGameItem(Collider2D coll)
    {
        PreGameItem preGameItem = coll.GetComponent<PreGameItem>();
        if (preGameItem != null)
        {
            currentPreGameDraggingItem = preGameItem;
            currentPreGameDraggingItem.OnStartDrag(top, currentMousePosition);
            return true;
        }
        return false;
    }

    private void StartCameraDrag()
    {
        if (canMoveCamera)
        {
            isDraggingCamera = true;
            lastScreenPosition = Input.mousePosition;
        }
    }

    private void HandleDragging()
    {
        if (isDraggingCamera)
        {
            cameraController.MoveCamera(ref lastScreenPosition);
        }
        else if (currentDrawingItem != null)
        {
            // Logic vẽ
            mouseDelta = currentMousePosition - prevMousePosition;
            currentDrawingItem.transform.position += mouseDelta;
            
            // Vẽ tại vị trí hiện tại (có thể thêm offset nếu cần)
            currentDrawingItem.DrawAtPosition(currentDrawingItem.transform.position);
            
            // Apply changes với interval để tối ưu
            if (Time.time - lastDrawApplyTime >= drawApplyInterval)
            {
                currentDrawingItem.ApplyMaskChanges();
                lastDrawApplyTime = Time.time;
            }
            
            prevMousePosition = currentMousePosition;
        }
        else if (currentDraggingItem != null)
        {
            mouseDelta = currentMousePosition - prevMousePosition;
            currentDraggingItem.OnDrag(mouseDelta, left, right, bottom, top);
            prevMousePosition = currentMousePosition;
        }
        else if (currentPreGameDraggingItem != null)
        {
            mouseDelta = currentMousePosition - prevMousePosition;
            currentPreGameDraggingItem.OnDrag(mouseDelta, left, right, bottom, top);
            prevMousePosition = currentMousePosition;
        }
    }

    private void HandleMouseUp()
    {
        if (currentDrawingItem != null)
        {
            currentDrawingItem.ApplyMaskChanges();
            
            if (currentDrawingItem.CheckDrawingCoverage())
            {
                OnDrawingStageComplete();
            }
            
            currentDrawingItem = null;
        }
        else if (currentDraggingItem != null)
        {
            currentDraggingItem.OnEndDrag(1f);
            currentDraggingItem = null;
        }

        UpdateBounds();
        isDraggingCamera = false;
    }
    
    private void OnDrawingStageComplete()
    {
        Debug.Log("Drawing stage completed!");
    }

    private void UpdateBounds()
    {
        left = playerContains.left.transform.position.x;
        top = playerContains.top.transform.position.y;
        right = playerContains.right.transform.position.x;
        bottom = GameController.Instance.useProfile.IsRemoveAds
            ? playerContains.bottom.transform.position.y
            : playerContains.bottom.transform.position.y + 2f;
    }

    public void SetWin(bool state) => isWin = state;
    public void SetLose(bool state) => isLose = state;
    public void SetPopupState(bool state)
    {
        isPopupOpen = state;
    
        // Reset trạng thái khi tắt popup
        if (!state)
        {
            isDraggingCamera = false;
            currentDraggingItem = null;
            currentPreGameDraggingItem = null;
            currentDrawingItem = null; // Reset drawing item
            
            lastScreenPosition = Input.mousePosition;
            currentMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            currentMousePosition.z = 0;
            prevMousePosition = currentMousePosition;
        }
    }
    public void SetCanMoveCamera(bool state) => canMoveCamera = state;
    
    public ItemBase GetCurrentDraggingItem() => currentDraggingItem;
    public ItemRemoverBase GetCurrentDrawingItem() => currentDrawingItem;
}