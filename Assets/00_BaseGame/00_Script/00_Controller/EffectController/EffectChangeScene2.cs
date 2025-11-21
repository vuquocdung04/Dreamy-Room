using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EffectChangeScene2 : MonoBehaviour
{
    public Transform parent;
    public SpriteRenderer imgBg;
    public SpriteRenderer imgIcon;
    public SpriteMask imgMask;
    
    [Header("Effect Settings")]
    public float durationFadeIn = 1f;
    public float durationFadeOut = 0.5f;
    
    [Header("Camera Settings")]
    public float defaultOrthoSize = 5f; 

    public List<Sprite> lsSpritesBg;
    private DataLevelBase dataLevel;

    public void Init()
    {
        dataLevel = GameController.Instance.dataContains.dataLevel;
    }

    private void SetImage()
    {
        int currentLevel = UseProfile.CurrentLevel;
        var icon = dataLevel.GetLevelSpriteById(currentLevel);
        
        if (lsSpritesBg != null && lsSpritesBg.Count > 0)
        {
            int rand = UnityEngine.Random.Range(0, lsSpritesBg.Count);
            imgBg.sprite = lsSpritesBg[rand];
        }
        
        imgIcon.sprite = icon;
        imgMask.sprite = icon;
    }

    public void ChangeScene(string sceneName)
    {
        RunEffect(sceneName).Forget();
    }
    
    /// <summary>
    /// Hàm tính toán Scale không phụ thuộc cứng vào Camera.main
    /// </summary>
    private float CalculateCoverScale(Sprite sprite)
    {
        if (sprite == null) return 25f;
        
        Camera currentCam = Camera.main;
        if (currentCam == null)
        {
            Debug.Log("currentCam is null");
            currentCam = FindFirstObjectByType<Camera>();
        }

        // 2. Lấy Orthographic Size (chiều cao camera)
        // Nếu tìm được cam thì lấy size của cam, nếu không thì dùng số mặc định bạn điền
        float orthoSize = (currentCam != null) ? currentCam.orthographicSize : defaultOrthoSize;

        // 3. Tính chiều cao và rộng màn hình theo World Unit
        float screenHeightWorld = orthoSize * 2;
        
        // Dùng Screen.width/height để tính tỷ lệ màn hình (cái này luôn đúng kể cả không có cam)
        float screenAspect = (float)Screen.width / Screen.height;
        float screenWidthWorld = screenHeightWorld * screenAspect;

        // 4. Lấy kích thước Sprite
        float spriteHeight = sprite.bounds.size.y;
        float spriteWidth = sprite.bounds.size.x;

        // Tránh chia cho 0
        if (spriteHeight == 0 || spriteWidth == 0) return 25f; 

        // 5. Tính Scale cần thiết
        float scaleY = screenHeightWorld / spriteHeight;
        float scaleX = screenWidthWorld / spriteWidth;

        // Chọn tỉ lệ lớn hơn + hệ số an toàn 2 lần để chắc chắn che kín
        return Mathf.Max(scaleX, scaleY) * 2f; 
    }

    private async UniTask RunEffect(string sceneName)
    {
        parent.gameObject.SetActive(true);
        SetImage();
        
        float targetScale = CalculateCoverScale(imgIcon.sprite);
        
        imgIcon.transform.localScale = Vector3.one * targetScale;
        
        await imgIcon.transform.DOScale(Vector3.zero, durationFadeIn).SetEase(Ease.Linear).AsyncWaitForCompletion();
        
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
        await loadOperation;
        
        await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
        
        if (sceneName.Equals(SceneName.GAME_PLAY))
            GamePlayController.Instance.PauseGame();
        
        await imgIcon.transform.DOScale(Vector3.one * targetScale, durationFadeOut).SetEase(Ease.Linear)
            .AsyncWaitForCompletion();
            
        parent.gameObject.SetActive(false);
        
        if (sceneName.Equals(SceneName.GAME_PLAY))
            GamePlayController.Instance.InitEffect();
    }
}