using System;
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
    [SerializeField] private Image frozeTiming;
    private CameraController cameraController;

    [Header("Time Setting")]
    private float totalTime;
    [SerializeField] private float currentTime;
    [SerializeField] private bool isTimerRunning;

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
        frozeTiming.gameObject.SetActive(true);
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