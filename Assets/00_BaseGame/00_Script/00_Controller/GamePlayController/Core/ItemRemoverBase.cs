using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ItemRemoverStorage
{
    public SpriteRenderer targetSprite;
    public Transform stainTrans;
    public SpriteMask maskDraw;
    
    [HideInInspector] public Texture2D maskTex;
    [HideInInspector] public Sprite maskSprite;
    [HideInInspector] public Color[] pixels;
    [HideInInspector] public int drawnPixels;
    [HideInInspector] public int width, height;
    [HideInInspector] public float ppu;
}

public class ItemRemoverBase : MonoBehaviour
{
    [Header("Stages")]
    [SerializeField] List<ItemRemoverStorage> stages;

    [Header("Brush")]
    [SerializeField] Sprite brushSprite;
    [SerializeField] float brushScale = 1f;
    [SerializeField] Color drawColor = Color.white;

    [Header("Complete")]
    [Range(0.01f, 1f)] [SerializeField] float completePercent = 0.9f;

    int currentStage;
    Color[] brushPixels;
    int brushW, brushH;
    Vector2 brushPivot;

    private void Awake()
    {
        InitBrush();
    }

    private void InitBrush()
    {
        if (brushSprite == null) { Debug.LogError("Brush Sprite chưa assign!"); return; }

        var tex = brushSprite.texture;
        var rect = brushSprite.rect;
        brushPixels = tex.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
        brushW = (int)rect.width;
        brushH = (int)rect.height;
        brushPivot = brushSprite.pivot;
    }

    public virtual void Init()
    {
        foreach (var s in stages)
        {
            s.stainTrans?.gameObject.SetActive(false);
            if (s.maskDraw) { s.maskDraw.enabled = false; s.maskDraw.sprite = null; }
        }
        currentStage = 0;
        SetupCurrentStage();
    }

    private void SetupCurrentStage()
    {
        if (currentStage >= stages.Count)
        {
            OnAllStagesComplete();
            return;
        }

        var s = stages[currentStage];
        s.stainTrans?.gameObject.SetActive(true);

        var target = s.targetSprite.sprite;
        s.width = (int)target.rect.width;
        s.height = (int)target.rect.height;
        s.ppu = target.pixelsPerUnit;

        // Tạo mask texture
        if (s.maskTex) Destroy(s.maskTex);
        s.maskTex = new Texture2D(s.width, s.height, TextureFormat.Alpha8, false);
        s.maskTex.SetPixels32(new Color32[s.width * s.height]); // clear alpha = 0
        s.maskTex.Apply();

        if (s.maskSprite) Destroy(s.maskSprite);
        s.maskSprite = Sprite.Create(s.maskTex, new Rect(0,0,s.width,s.height), Vector2.one*0.5f, s.ppu);

        s.maskDraw.sprite = s.maskSprite;
        s.maskDraw.enabled = true;
        s.maskDraw.transform.SetParent(s.targetSprite.transform, false);

        s.pixels = s.maskTex.GetPixels();
        s.drawnPixels = 0;
    }

    public void DrawAtPosition(Vector3 worldPos)
    {
        if (currentStage >= stages.Count) return;
        var s = stages[currentStage];

        Vector3 local = s.maskDraw.transform.InverseTransformPoint(worldPos);
        Vector2 norm = new Vector2(
            local.x / (s.targetSprite.sprite.rect.width / s.ppu) + 0.5f,
            local.y / (s.targetSprite.sprite.rect.height / s.ppu) + 0.5f);

        int cx = Mathf.RoundToInt(norm.x * s.width);
        int cy = Mathf.RoundToInt(norm.y * s.height);

        if (cx < 0 || cy < 0 || cx >= s.width || cy >= s.height) return;

        DrawBrush(s, cx, cy);
        ApplyIfNeeded();
    }

    private void DrawBrush(ItemRemoverStorage s, int centerX, int centerY)
    {
        int w = Mathf.RoundToInt(brushW * brushScale);
        int h = Mathf.RoundToInt(brushH * brushScale);
        Vector2 pivot = brushPivot * brushScale;

        int startX = centerX - Mathf.RoundToInt(pivot.x);
        int startY = centerY - Mathf.RoundToInt(pivot.y);

        for (int y = 0; y < h; y++)
        {
            int ty = startY + y;
            if (ty < 0 || ty >= s.height) continue;

            for (int x = 0; x < w; x++)
            {
                int tx = startX + x;
                if (tx < 0 || tx >= s.width) continue;

                // Sample gốc bằng nearest neighbor
                int bx = Mathf.Min((int)(x / brushScale), brushW - 1);
                int by = Mathf.Min((int)(y / brushScale), brushH - 1);
                if (brushPixels[by * brushW + bx].a < 0.01f) continue;

                int i = ty * s.width + tx;
                if (s.pixels[i].a < 0.01f) s.drawnPixels++;
                s.pixels[i] = drawColor;
            }
        }
    }

    float lastApplyTime;
    [SerializeField] float applyInterval = 0.05f;

    private void ApplyIfNeeded()
    {
        if (Time.time - lastApplyTime < applyInterval) return;
        lastApplyTime = Time.time;

        var s = stages[currentStage];
        s.maskTex.SetPixels(s.pixels);
        s.maskTex.Apply();

        // Check hoàn thành
        float coverage = (float)s.drawnPixels / (s.width * s.height);
        if (coverage >= completePercent)
            NextStage();
    }

    public void ForceApplyAndCheck()
    {
        if (currentStage >= stages.Count) return;
        var s = stages[currentStage];
        s.maskTex.SetPixels(s.pixels);
        s.maskTex.Apply();

        float coverage = (float)s.drawnPixels / (s.width * s.height);
        if (coverage >= completePercent)
            NextStage();
    }

    void NextStage()
    {
        var s = stages[currentStage];
        s.stainTrans?.gameObject.SetActive(false);
        s.maskDraw.enabled = false;

        currentStage++;
        SetupCurrentStage();
    }

    protected virtual void OnAllStagesComplete()
    {
        Debug.Log("Hoàn thành tất cả stage!");
    }
}