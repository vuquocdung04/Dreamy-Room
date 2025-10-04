using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField] private Button btnPause;
    [SerializeField] private Button btnZoom;
    [SerializeField] private Button btnRemoveAds;

    [SerializeField] private Image imgZoom;
    [SerializeField] private Sprite sprZoomIn;
    [SerializeField] private Sprite sprZoomOut;

    [SerializeField] private RectTransform topBar;

    [SerializeField] private TextMeshProUGUI txtTiming;
    [SerializeField] private Image imgFrozeTimer;
    [SerializeField] private Image imgFrozeBg;
    private CameraController cameraController;

    [Header("Time Setting")] [SerializeField]
    private float totalTime;

    [SerializeField] private float currentTime;
    [SerializeField] private bool isTimerRunning;

    [Header("Froze Setting")] [SerializeField]
    private float frozeRemainingTime;

    [SerializeField] private Image fillFrozeReload;
    [SerializeField] private bool isFrozeActive;
    [SerializeField] private float frozeDuration = 30f;

    [Header("Pause Setting")] [SerializeField]
    private bool isPaused; // Pause từ popup

    private bool hasUsedTimeOffer;

    public void Init()
    {
        cameraController = GamePlayController.Instance.playerContains.cameraController;

        btnPause.onClick.AddListener(delegate
        {
            GamePlayController.Instance.PauseGame();
            SettingGameBox.Setup().Show();
        });
        btnRemoveAds.onClick.AddListener(delegate
        {
            GamePlayController.Instance.PauseGame();
            RemoveAdsBox.Setup().Show();
        });

        btnZoom.onClick.AddListener(HandleToggleZoomInOut);

        StartTimer(300);
    }

    private void StartTimer(float seconds)
    {
        totalTime = seconds;
        currentTime = seconds;
        isTimerRunning = true;
        
        UpdateTimerDisplay();
    }

    private void Update()
    {
        // Nếu đang pause thì không làm gì cả
        if (isPaused) return;

        if (isFrozeActive)
        {
            frozeRemainingTime -= Time.deltaTime;
            if (frozeRemainingTime <= 0)
                EndFroze();
            else
            {
                fillFrozeReload.fillAmount = frozeRemainingTime/ frozeDuration;
            }
            return;
        }

        // Xử lý Main Timer
        if (isTimerRunning && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                isTimerRunning = false;
                OnTimerComplete();
            }

            UpdateTimerDisplay();
        }
    }
    
    public void ActivateFrozeBooster()
    {
        if (isFrozeActive) return; // Đang froze rồi thì không cho dùng nữa

        isFrozeActive = true;
        frozeRemainingTime = frozeDuration;
        imgFrozeTimer.DOFillAmount(1f, 0.2f);
        imgFrozeBg.DOFade(1f, 0.2f);
    }

    private void EndFroze()
    {
        isFrozeActive = false;
        frozeRemainingTime = 0;
        imgFrozeBg.DOFade(0f, 0.2f).SetEase(Ease.InOutSine);
        imgFrozeTimer.DOFillAmount(0f, 0.2f);
    }

    private void HandleToggleZoomInOut()
    {
        cameraController.ToggleZoomInOut();
        var isZoomed = cameraController.GetIsZoomed();
        imgZoom.sprite = isZoomed ? sprZoomOut : sprZoomIn;
    }

    #region Time Control

    /// <summary>
    /// Pause cả Main Timer và Froze Timer (dùng khi mở popup)
    /// </summary>
    [Button("Pause (Popup)", ButtonSizes.Large)]
    public void PauseTime()
    {
        isPaused = true;
    }

    /// <summary>
    /// Resume cả Main Timer và Froze Timer (dùng khi đóng popup)
    /// </summary>
    [Button("Resume (Close Popup)", ButtonSizes.Large)]
    public void ResumeTime()
    {
        isPaused = false;
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        txtTiming.text = $"{minutes:00}:{seconds:00}";
    }

    public void AddTime()
    {
        isTimerRunning = true;
        currentTime = 60;
    }

    private void OnTimerComplete()
    {
        if (!hasUsedTimeOffer)
        {
            TimeOutBox.Setup().Show();
            hasUsedTimeOffer = true;
        }
        else
            LoseBox.Setup().Show();
        GamePlayController.Instance.PauseGame();
    }

    #endregion
}