
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

    [SerializeField] private Image fillTimingBar;
    [SerializeField] private TextMeshProUGUI txtTiming;
    [SerializeField] private Image imgFroze;
    private CameraController cameraController;

    [Header("Time Setting")]
    private float totalTime;
    [SerializeField] private float currentTime;
    [SerializeField] private bool isTimerRunning;

    [Header("Froze Setting")]
    [SerializeField] private float frozeRemainingTime;

    [SerializeField] private bool isFrozeActive;
    [SerializeField] private float frozeDuration = 30f;
    public void Init()
    {
        cameraController = GamePlayController.Instance.playerContains.cameraController;

        btnPause.onClick.AddListener(delegate { SettingGameBox.Setup().Show(); });
        btnRemoveAds.onClick.AddListener(delegate { RemoveAdsBox.Setup().Show(); });

        btnZoom.onClick.AddListener(HandleToggleZoomInOut);
        
        StartTimer(300);
    }

    private void StartTimer(float seconds)
    {
        totalTime = seconds;
        currentTime = seconds;
        isTimerRunning = true;

        fillTimingBar.fillAmount = 1f;
        UpdateTimerDisplay();
    }
    
    private void Update()
    {
        if (isFrozeActive)
        {
            frozeRemainingTime -= Time.deltaTime;
            if (frozeRemainingTime <= 0)
                EndFroze();
            return;
        }
        
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
            UpdateFillBar();
        }
    }
    [Button("Active Froze", ButtonSizes.Large)]
    public void ActivateFrozeBooster()
    {
        isFrozeActive = true;
        frozeRemainingTime = frozeDuration;
        imgFroze.DOFillAmount(1f, 0.2f);
    }
    
    private void EndFroze()
    {
        isFrozeActive = false;
        frozeRemainingTime = 0;
        imgFroze.DOFillAmount(0f, 0.2f);
        ResumeTimer();
    }
    private void HandleToggleZoomInOut()
    {
        cameraController.ToggleZoomInOut();
        var isZoomed = cameraController.GetIsZoomed();
        imgZoom.sprite = isZoomed ? sprZoomOut : sprZoomIn;
    }

    #region Time
    
    [Button("Test", ButtonSizes.Large)]
    public void PauseTimer()
    {
        isTimerRunning = false;
    }

    public void ResumeTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
        currentTime = 0;
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        txtTiming.text = $"{minutes:00}:{seconds:00}";
    }

    private void UpdateFillBar()
    {
        float fillAmount = currentTime / totalTime;
        fillTimingBar.fillAmount = fillAmount;
    }

    private void OnTimerComplete()
    {
    }
    #endregion
}