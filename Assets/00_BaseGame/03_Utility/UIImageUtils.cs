using UnityEngine;
using UnityEngine.UI;

public static class UIImageUtils
{
    public static void FitToTargetHeight(Image image, float targetHeight)
    {
        if (image == null || image.sprite == null)
            return;

        Rect rect = image.sprite.rect;
        float aspectRatio = rect.width / rect.height;
        float newWidth = targetHeight * aspectRatio;

        image.rectTransform.sizeDelta = new Vector2(newWidth, targetHeight);
    }
    
    public static void FitToTargetWidth(Image image, float targetWidth)
    {
        if (image == null || image.sprite == null)
            return;

        Rect rect = image.sprite.rect;
        float aspectRatio = rect.height / rect.width; // đảo lại để tính height
        float newHeight = targetWidth * aspectRatio;

        image.rectTransform.sizeDelta = new Vector2(targetWidth, newHeight);
    }

    public static void FitToSquareBase(Image image, float baseSize = 100f)
    {
        if (image == null || image.sprite == null)
            return;

        Rect rect = image.sprite.rect;
        float aspectRatio = rect.width / rect.height;

        float width, height;
        if (aspectRatio >= 1f)
        {
            width = baseSize;
            height = baseSize / aspectRatio;
        }
        else
        {
            height = baseSize;
            width = baseSize * aspectRatio;
        }

        image.rectTransform.sizeDelta = new Vector2(width, height);
    }
}