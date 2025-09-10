using UnityEngine;

public class CameraAspectFitter : MonoBehaviour
{
    [SerializeField] private float baseHeight = 10f; // Chiều cao hiển thị mong muốn (2 * orthographicSize)
    [SerializeField] private float baseWidth = 5.625f; // Tính từ 1080/1920 * baseHeight/2 * 2 -> 1080/1920 * 10 = 5.625

    [SerializeField] Camera cam;

    public void Init()
    {
        AdjustCamera();
    }

    void AdjustCamera()
    {
        float targetAspect = baseWidth / baseHeight; // Tỉ lệ aspect mong muốn (1080:1920 = 0.5625)
        float windowAspect = (float)Screen.width / Screen.height;

        // Tính orthographic size để giữ chiều cao cố định, mở rộng chiều ngang nếu cần
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1f)
        {
            // Màn hình "dày" hơn (ít dọc hơn), cần tăng size để thấy đủ chiều dọc
            cam.orthographicSize = baseHeight * 0.5f / scaleHeight;
        }
        else
        {
            // Màn hình chuẩn hoặc cao hơn, giữ nguyên size
            cam.orthographicSize = baseHeight * 0.5f;
        }
    }

    void OnRectTransformDimensionsChange()
    {
        AdjustCamera();
    }
}