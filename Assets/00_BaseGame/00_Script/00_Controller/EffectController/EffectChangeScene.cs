using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EffectChangeScene : MonoBehaviour
{
    [Header("Settings")]
    [Range(0.3f,2f)]
    public float duration;
    [InfoBox("Mau nen khi fade"), Space(5)]
    [ColorPalette]
    public Color fadeColor = Color.black;
    
    private static bool isTransitioning;

    private void ChangeScene(string sceneName)
    {
        if (isTransitioning) return;
        isTransitioning = true;

        // Tạo overlay
        GameObject overlay = CreateOverlay();
        Image img = overlay.GetComponent<Image>();
        img.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0);

        // Fade in -> load scene -> fade out
        img.DOFade(1, duration).OnComplete(() =>
        {
            SceneManager.LoadScene(sceneName);
            img.DOFade(0, duration).OnComplete(() =>
            {
                Destroy(overlay);
                isTransitioning = false;
            });
        });
    }

    private GameObject CreateOverlay()
    {
        GameObject obj = new GameObject("SceneTransition");
        Canvas canvas = obj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999;
        
        Image img = obj.AddComponent<Image>();
        img.color = fadeColor;
        
        RectTransform rect = img.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        
        return obj;
    }

    // Static method
    public void FadeToScene(string sceneName)
    {
        ChangeScene(sceneName);
    }
}