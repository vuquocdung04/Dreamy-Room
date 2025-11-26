using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemRemoverStorage
{
    public SpriteRenderer targetSprite;
    public Transform stainTrans;
    public SpriteMask maskDraw;
    [NonSerialized] public Texture2D MaskTexture;
    [NonSerialized] public Sprite MaskSprite;
    [NonSerialized] public Color[] MaskPixelsBuffer;
    [NonSerialized] public int DrawnPixelCount;
    [NonSerialized] public int TextureWidth;
    [NonSerialized] public int TextureHeight;
    [NonSerialized] public bool IsPercentReached;
    [NonSerialized] public float PixelsPerUnit;
    [NonSerialized] public Rect SpriteRect;
}

public class ItemRemoverBase : MonoBehaviour
{
    [Header("Stages")]
    [SerializeField] private List<ItemRemoverStorage> lsStages;
    
    [Header("Drawing Settings")]
    [SerializeField] private Sprite brushSprite; // Sprite hình dạng brush để vẽ (kéo thả vào Inspector)
    [SerializeField] private float brushScale = 1f; // Scale để tăng độ to của brush
    [SerializeField] private Color drawColor = Color.white;
    
    [Header("Coverage Settings")]
    [SerializeField] [Range(0f, 1f)] private float completePercent = 0.9f;
    
    private int currentStateIndex;
    [NonSerialized] private Color[] brushPixels;
    [NonSerialized] private int brushWidth;
    [NonSerialized] private int brushHeight;
    [NonSerialized] private int scaledBrushWidth;
    [NonSerialized] private int scaledBrushHeight;
    [NonSerialized] private Vector2 brushPivot;

    public virtual void Init()
    {
        // Preload brush pixels nếu có sprite
        if (brushSprite != null)
        {
            brushWidth = (int)brushSprite.rect.width;
            brushHeight = (int)brushSprite.rect.height;
            brushPivot = brushSprite.pivot;
            brushPixels = brushSprite.texture.GetPixels(
                (int)brushSprite.rect.x, 
                (int)brushSprite.rect.y, 
                brushWidth, 
                brushHeight
            );
            scaledBrushWidth = Mathf.CeilToInt(brushWidth * brushScale);
            scaledBrushHeight = Mathf.CeilToInt(brushHeight * brushScale);
        }
        else
        {
            Debug.LogWarning("Brush sprite chưa được assign! Sẽ không vẽ được.");
        }

        foreach (var stage in lsStages)
        {
            if (stage.stainTrans != null)
                stage.stainTrans.gameObject.SetActive(false);
            if (stage.maskDraw != null)
            {
                stage.maskDraw.enabled = false;
                stage.maskDraw.sprite = null;
            }
        }
        InitCurrentState();
    }

    private void InitCurrentState()
    {
        if (currentStateIndex >= lsStages.Count)
        {
            Debug.Log("Đã hoàn thành tất cả các stage!");
            OnAllStagesComplete();
            return;
        }
        var currentStage = lsStages[currentStateIndex];
        if (currentStage.stainTrans != null)
        {
            currentStage.stainTrans.gameObject.SetActive(true);
        }
       
        if (currentStage.maskDraw != null && currentStage.targetSprite != null)
        {
            currentStage.maskDraw.transform.SetParent(currentStage.targetSprite.transform);
            currentStage.maskDraw.transform.localPosition = Vector3.zero;
            currentStage.maskDraw.transform.localRotation = Quaternion.identity;
            currentStage.maskDraw.transform.localScale = Vector3.one;
        }
        Sprite targetSpriteRef = currentStage.targetSprite.sprite;
       
        currentStage.TextureWidth = (int)targetSpriteRef.rect.width;
        currentStage.TextureHeight = (int)targetSpriteRef.rect.height;
        currentStage.PixelsPerUnit = targetSpriteRef.pixelsPerUnit;
        currentStage.SpriteRect = targetSpriteRef.rect;
        if (currentStage.MaskTexture == null ||
            currentStage.MaskTexture.width != currentStage.TextureWidth ||
            currentStage.MaskTexture.height != currentStage.TextureHeight)
        {
            if (currentStage.MaskTexture != null) Destroy(currentStage.MaskTexture);
            // Sử dụng Alpha8 để tiết kiệm bộ nhớ, vì chỉ cần kênh alpha
            currentStage.MaskTexture = new Texture2D(currentStage.TextureWidth, currentStage.TextureHeight, TextureFormat.Alpha8, false);
        }
        ClearTexture(currentStage.MaskTexture, new Color(0, 0, 0, 0), currentStage.TextureWidth, currentStage.TextureHeight);
       
        if (currentStage.MaskSprite == null || currentStage.MaskSprite.texture != currentStage.MaskTexture)
        {
            if (currentStage.MaskSprite != null) Destroy(currentStage.MaskSprite);
            currentStage.MaskSprite = Sprite.Create(
                currentStage.MaskTexture,
                new Rect(0, 0, currentStage.TextureWidth, currentStage.TextureHeight),
                new Vector2(0.5f, 0.5f),
                currentStage.PixelsPerUnit
            );
        }
        currentStage.maskDraw.sprite = currentStage.MaskSprite;
        currentStage.maskDraw.enabled = true;
        // Lấy buffer pixel từ texture đã được clear
        currentStage.MaskPixelsBuffer = currentStage.MaskTexture.GetPixels();
        currentStage.DrawnPixelCount = 0;
        currentStage.IsPercentReached = false;
       
        ApplyMaskChanges();
    }

    private void ClearTexture(Texture2D texture, Color color, int width, int height)
    {
        Color[] clearColors = new Color[width * height];
        for (int i = 0; i < clearColors.Length; i++) clearColors[i] = color;
        texture.SetPixels(clearColors);
        texture.Apply();
    }

    private void DrawBrush(ItemRemoverStorage stage, Vector2Int center, Color color)
    {
        if (brushSprite == null || brushPixels == null) return;

        Vector2 scaledPivot = brushPivot * brushScale;
        int offsetX = center.x - Mathf.RoundToInt(scaledPivot.x);
        int offsetY = center.y - Mathf.RoundToInt(scaledPivot.y);

        for (int sx = 0; sx < scaledBrushWidth; sx++)
        {
            for (int sy = 0; sy < scaledBrushHeight; sy++)
            {
                // Nearest neighbor sampling
                float origX = sx / brushScale;
                float origY = sy / brushScale;
                int bx = Mathf.FloorToInt(origX);
                int by = Mathf.FloorToInt(origY);

                if (bx >= 0 && bx < brushWidth && by >= 0 && by < brushHeight)
                {
                    Color bCol = brushPixels[by * brushWidth + bx];
                    if (bCol.a <= 0.01f) continue; // Bỏ qua pixel trong suốt trong brush

                    int tx = offsetX + sx;
                    int ty = offsetY + sy;

                    if (tx >= 0 && tx < stage.TextureWidth && ty >= 0 && ty < stage.TextureHeight)
                    {
                        int index = ty * stage.TextureWidth + tx;
                        // Chỉ tăng count nếu pixel trước đó trong suốt
                        if (stage.MaskPixelsBuffer[index].a <= 0.01f && color.a > 0.01f)
                        {
                            stage.DrawnPixelCount++;
                        }
                        // Set hard như circle, không blend để giữ hiệu năng và giống logic cũ
                        // Nếu muốn soft edge, có thể thay bằng: stage.MaskPixelsBuffer[index].a = Mathf.Max(stage.MaskPixelsBuffer[index].a, color.a * bCol.a);
                        // Nhưng để giống circle (hard), set trực tiếp nếu brush alpha > threshold
                        stage.MaskPixelsBuffer[index] = color;
                    }
                }
            }
        }
    }
   
    public void ApplyMaskChanges()
    {
        if (currentStateIndex >= lsStages.Count) return;
        var currentStage = lsStages[currentStateIndex];
        if (currentStage.MaskTexture != null)
        {
            currentStage.MaskTexture.SetPixels(currentStage.MaskPixelsBuffer);
            currentStage.MaskTexture.Apply();
        }
    }

    public bool CheckDrawingCoverage()
    {
        if (currentStateIndex >= lsStages.Count) return false;
        var currentStage = lsStages[currentStateIndex];
        if (currentStage.IsPercentReached) return false;
        float totalPixels = (float)currentStage.TextureWidth * currentStage.TextureHeight;
        if (totalPixels == 0) return false;
        float coverage = currentStage.DrawnPixelCount / totalPixels;
        if (coverage >= completePercent)
        {
            currentStage.IsPercentReached = true;
            AdvanceToNextStage();
            return true;
        }
        return false;
    }

    private void AdvanceToNextStage()
    {
        if (currentStateIndex >= lsStages.Count) return;
        var completedStage = lsStages[currentStateIndex];
        if (completedStage.stainTrans != null)
        {
            completedStage.stainTrans.gameObject.SetActive(false);
        }
        if (completedStage.maskDraw != null)
        {
            completedStage.maskDraw.enabled = false;
            completedStage.maskDraw.sprite = null;
        }
        currentStateIndex++;
        InitCurrentState();
    }
   
    public void DrawAtPosition(Vector3 worldPos)
    {
        if (currentStateIndex >= lsStages.Count) return;
        var currentStage = lsStages[currentStateIndex];
        if (currentStage.IsPercentReached) return;
        Vector3 localPos = currentStage.maskDraw.transform.InverseTransformPoint(worldPos);
        float texXNormalized = (localPos.x / (currentStage.SpriteRect.width / currentStage.PixelsPerUnit)) + 0.5f;
        float texYNormalized = (localPos.y / (currentStage.SpriteRect.height / currentStage.PixelsPerUnit)) + 0.5f;
        int texX = (int)(texXNormalized * currentStage.TextureWidth);
        int texY = (int)(texYNormalized * currentStage.TextureHeight);
        if (texX >= 0 && texX < currentStage.TextureWidth && texY >= 0 && texY < currentStage.TextureHeight)
        {
            DrawBrush(currentStage, new Vector2Int(texX, texY), drawColor);
        }
    }
   
    protected virtual void OnAllStagesComplete()
    {
        Debug.Log("All stages completed! Override this method for custom behavior.");
        // Ví dụ: Gọi hàm Win ở đây
    }
}

[Serializable]
public struct Vector2Int
{
    public int x;
    public int y;

    public Vector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}