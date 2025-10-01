using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Camera mainCamera;
    private CameraController cameraController;

    private bool isDraggingCamera;
    private ItemBase currentDraggingItem;

    // Cho camera dragging (dùng screen space)
    private Vector3 lastScreenPosition;

    // Cho object dragging (dùng world space)
    private Vector3 currentMousePosition;
    private Vector3 prevMousePosition;
    private Vector3 delta;
    private float left, top, right, bottom;

    private PlayerContains playerContains;

    [SerializeField] private bool isWin, isLose, isPopupOpen, canMoveCamera;

    public void Init()
    {
        playerContains = GamePlayController.Instance.playerContains;
        mainCamera = playerContains.mainCamera;
        cameraController = playerContains.cameraController;
        UpdateBonds();

        //canMoveCamera = UseProfile.MaxUnlockedLevel > 5;
    }

    private void Update()
    {
        if (isWin || isPopupOpen || isLose) return;


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
                if (canMoveCamera)
                {
                    isDraggingCamera = true;
                    lastScreenPosition = Input.mousePosition;
                }
            }
            else
            {
                BoxGameBase box = hit.collider.GetComponent<BoxGameBase>();
                if (box != null)
                {
                    box.OnBoxClicked();
                    return;
                }

                ItemBase item = hit.collider.GetComponent<ItemBase>();
                if (item != null)
                {
                    currentDraggingItem = item;
                    currentDraggingItem.OnStartDrag(top, currentMousePosition);
                }
                else
                {
                    if (canMoveCamera)
                    {
                        isDraggingCamera = true;
                        lastScreenPosition = Input.mousePosition;
                    }
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
            currentDraggingItem.OnDrag(delta, left, right, bottom, top);
            prevMousePosition = currentMousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (currentDraggingItem != null)
            {
                float snapThreshold = 1f;
                currentDraggingItem.OnEndDrag(snapThreshold);
                currentDraggingItem = null;
            }

            UpdateBonds();
            isDraggingCamera = false;
        }
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

    public void SetWin(bool state)
    {
        isWin = state;
    }

    public void SetLose(bool state)
    {
        isLose = state;
    }

    public void SetPopupState(bool state)
    {
        isPopupOpen = state;
    }

    public void SetCanMoveCamera(bool state)
    {
        canMoveCamera = state;
    }
}