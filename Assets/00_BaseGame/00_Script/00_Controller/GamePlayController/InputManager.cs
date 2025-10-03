using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Camera mainCamera;
    private CameraController cameraController;
    private PlayerContains playerContains;

    private bool isDraggingCamera;
    private ItemBase currentDraggingItem;
    private PreGameItem currentPreGameDraggingItem;

    private Vector3 lastScreenPosition;
    private Vector3 currentMousePosition;
    private Vector3 prevMousePosition;
    private Vector3 mouseDelta;
    private float left, top, right, bottom;

    [SerializeField] private bool isWin, isLose, isPopupOpen, canMoveCamera;

    public void Init()
    {
        playerContains = GamePlayController.Instance.playerContains;
        mainCamera = playerContains.mainCamera;
        cameraController = playerContains.cameraController;
        UpdateBonds();
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
        RaycastHit2D hit = Physics2D.Raycast(currentMousePosition, Vector2.zero);
        if (hit.collider == null)
        {
            StartCameraDrag();
            return;
        }

        // Thử các component theo thứ tự ưu tiên
        if (TryHandleBox(hit.collider)) return;
        if (TryHandleItem(hit.collider)) return;
        if (TryHandlePreGameItem(hit.collider)) return;
        
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
        if (currentDraggingItem != null)
        {
            currentDraggingItem.OnEndDrag(1f);
            currentDraggingItem = null;
        }

        UpdateBonds();
        isDraggingCamera = false;
    }

    private void UpdateBonds()
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
    public void SetPopupState(bool state) => isPopupOpen = state;
    public void SetCanMoveCamera(bool state) => canMoveCamera = state;
}