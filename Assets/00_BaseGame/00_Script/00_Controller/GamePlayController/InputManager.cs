
using UnityEngine;
public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    
    private bool isDraggingCamera;
    private Vector3 lastScreenPosition; // Dùng screen position thay vì world position
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            
            if (hit.collider == null)
            {
                isDraggingCamera = true;
                lastScreenPosition = Input.mousePosition; // Lưu screen position
            }
        }

        if (isDraggingCamera)
        {
            MoveCamera();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDraggingCamera = false;
        }
    }

    private void MoveCamera()
    {
        Vector3 currentScreenPosition = Input.mousePosition;
        Vector3 deltaScreen = currentScreenPosition - lastScreenPosition;
        
        // Chuyển đổi delta từ screen space sang world space
        Vector3 deltaWorld = mainCamera.ScreenToWorldPoint(deltaScreen) - mainCamera.ScreenToWorldPoint(Vector3.zero);
        
        mainCamera.transform.position -= new Vector3(deltaWorld.x, 0, 0);
        lastScreenPosition = currentScreenPosition;
    }
}