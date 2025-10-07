using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemRemoverStorage
{
    public SpriteRenderer targetSprite; // lay with va height
    public Transform stainTrans; // vet van
    public SpriteMask maskDraw; // mask can xoa

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

    [Header("Stamp Settings")] 
    [SerializeField] private Sprite stampSprite;
    [SerializeField] private Color drawColor = Color.white;
    
    [Header("Coverage Settings")]
    [SerializeField] [Range(0f, 1f)] private float completePercent = 0.9f;

    private int currentStateIndex;

    //Cache Data
    private Texture2D stampTexture;
    private Color[] stampPixels;
    private int stampWidth;
    private int stampHeight;
    private int stampHaftWidth;
    private int stampHaftHeight;

    private void Start()
    {
        // Cache stamp data
        if (stampSprite != null)
        {
            stampTexture = stampSprite.texture;
            Rect rect = stampSprite.rect;
            stampWidth = (int)rect.width;
            stampHeight = (int)rect.height;
            stampHaftWidth = stampWidth / 2;
            stampHaftHeight = stampHeight / 2;

            stampPixels = stampTexture.GetPixels(
                (int)rect.x,
                (int)rect.y,
                stampWidth,
                stampHeight
            );
            
            Debug.Log($"Stamp loaded: {stampWidth}x{stampHeight} pixels");
        }
        else
        {
            Debug.LogError("Stamp Sprite is null! Please assign a sprite.");
        }

        // Tat tat ca stage
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
        // Hoan thanh thi thoi
        if (currentStateIndex >= lsStages.Count)
        {
            Debug.Log("Đã hoàn thành tất cả các stage!");
            OnAllStagesComplete();
            return;
        }

        var currentStage = lsStages[currentStateIndex];

        // Bat stain hien tai
        if (currentStage.stainTrans != null)
        {
            currentStage.stainTrans.gameObject.SetActive(true);
            Debug.Log($"Stage {currentStateIndex}: Đã bật stain");
        }

        Sprite targetSpriteRef = currentStage.targetSprite.sprite;
        Texture2D originalTex = targetSpriteRef.texture;

        currentStage.TextureWidth = originalTex.width;
        currentStage.TextureHeight = originalTex.height;
        currentStage.PixelsPerUnit = targetSpriteRef.pixelsPerUnit;
        currentStage.SpriteRect = targetSpriteRef.rect;

        // Tao mask texture
        if (currentStage.MaskTexture == null || 
            currentStage.MaskTexture.width != currentStage.TextureWidth ||
            currentStage.MaskTexture.height != currentStage.TextureHeight)
        {
            if (currentStage.MaskTexture != null)
                Destroy(currentStage.MaskTexture);

            currentStage.MaskTexture = new Texture2D(
                currentStage.TextureWidth,
                currentStage.TextureHeight,
                TextureFormat.RGBA32,
                false
            );
        }

        ClearTexture(currentStage.MaskTexture, new Color(0, 0, 0, 0), 
                    currentStage.TextureWidth, currentStage.TextureHeight);

        // Tao mask sprite
        if (currentStage.MaskSprite == null || currentStage.MaskSprite.texture != currentStage.MaskTexture)
        {
            if (currentStage.MaskSprite != null)
                Destroy(currentStage.MaskSprite);

            currentStage.MaskSprite = Sprite.Create(
                currentStage.MaskTexture,
                new Rect(0, 0, currentStage.TextureWidth, currentStage.TextureHeight),
                new Vector2(0.5f, 0.5f),
                currentStage.PixelsPerUnit
            );
        }

        currentStage.maskDraw.sprite = currentStage.MaskSprite;
        currentStage.maskDraw.enabled = true;

        currentStage.MaskPixelsBuffer = currentStage.MaskTexture.GetPixels();
        currentStage.DrawnPixelCount = 0;
        currentStage.IsPercentReached = false;

        ApplyMaskChanges();
    }

    private void ClearTexture(Texture2D texture, Color color, int width, int height)
    {
        Color[] clearColors = new Color[width * height];
        for (int i = 0; i < clearColors.Length; i++)
            clearColors[i] = color;
        texture.SetPixels(clearColors);
    }

    private void DrawStamp(ItemRemoverStorage stage, Vector2Int center)
    {
        if (stampPixels == null || stampPixels.Length == 0) return;

        int startX = center.x - stampHaftWidth;
        int startY = center.y - stampHaftHeight;

        for (int sy = 0; sy < stampHeight; sy++)
        {
            for (int sx = 0; sx < stampWidth; sx++)
            {
                int targetX = startX + sx;
                int targetY = startY + sy;

                // Kiem tra bounds
                if (targetX < 0 || targetX >= stage.TextureWidth ||
                    targetY < 0 || targetY >= stage.TextureHeight)
                    continue;

                int stampIndex = sy * stampWidth + sx;
                Color stampColor = stampPixels[stampIndex];

                // Chi ve pixel co alpha > 0
                if (stampColor.a > 0.01f)
                {
                    int targetIndex = targetY * stage.TextureWidth + targetX;

                    // Dem pixel moi duoc ve
                    if (stage.MaskPixelsBuffer[targetIndex].a <= 0.01f)
                        stage.DrawnPixelCount++;

                    // Ap dung mau
                    stage.MaskPixelsBuffer[targetIndex] = new Color(
                        drawColor.r,
                        drawColor.g,
                        drawColor.b,
                        stampColor.a
                    );
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

        float totalPixels = currentStage.TextureWidth * currentStage.TextureHeight;
        float coverage = currentStage.DrawnPixelCount / totalPixels;

        if (coverage >= completePercent)
        {
            currentStage.IsPercentReached = true;
            Debug.Log($"Stage {currentStateIndex}: Đạt {coverage * 100:F1}% độ phủ!");
            AdvanceToNextStage();
            return true;
        }
        
        return false;
    }

    private void AdvanceToNextStage()
    {
        if (currentStateIndex >= lsStages.Count) return;

        var completedStage = lsStages[currentStateIndex];

        // Tat stain va mask cua stage vua hoan thanh
        if (completedStage.stainTrans != null)
        {
            completedStage.stainTrans.gameObject.SetActive(false);
            Debug.Log($"Stage {currentStateIndex}: Đã tắt stain");
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

        // Chuyen world pos sang local pos
        Vector3 localPos = currentStage.maskDraw.transform.InverseTransformPoint(worldPos);

        // Tinh toa do texture
        float texXNormalized = (localPos.x / (currentStage.SpriteRect.width / currentStage.PixelsPerUnit)) + 0.5f;
        float texYNormalized = (localPos.y / (currentStage.SpriteRect.height / currentStage.PixelsPerUnit)) + 0.5f;

        int texX = (int)(texXNormalized * currentStage.TextureWidth);
        int texY = (int)(texYNormalized * currentStage.TextureHeight);

        // Kiem tra bounds
        if (texX >= 0 && texX < currentStage.TextureWidth &&
            texY >= 0 && texY < currentStage.TextureHeight)
        {
            DrawStamp(currentStage, new Vector2Int(texX, texY));
        }
    }

    // Virtual methods de override
    protected virtual void OnAllStagesComplete()
    {
        Debug.Log("All stages completed! Override this method for custom behavior.");
    }
}