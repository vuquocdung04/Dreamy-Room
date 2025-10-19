using DG.Tweening;
using EventDispatcher;
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
    [SerializeField] private RectTransform bottomBar;
    [SerializeField] private RectTransform boosterBar;
    [SerializeField] private RectTransform starBar;
    [SerializeField] private TextMeshProUGUI txtStar;
    [SerializeField] private RectTransform swipeCamera;
    
    [SerializeField] private Image fillProgressBar;
    [SerializeField] private TextMeshProUGUI txtTiming;
    [SerializeField] private Image imgFrozeTimer;
    [SerializeField] private Image imgFrozeBg;
    private CameraController cameraController;

    [Header("Time Setting")]

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
    private int starAmount;
    public void Init()
    {
        txtStar.text = starAmount.ToString();
        
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
    public RectTransform GetStarBar() => starBar;
    public int GetStarAmount() => starAmount;
    public void IncreaseStarAmount()
    {
        starAmount++;
        txtStar.text = starAmount.ToString();
    }

    public void DisplaySwipeCam()
    {
        swipeCamera.gameObject.SetActive(true);
    }
    public void DisplayTopBar()
    {
        topBar.gameObject.SetActive(true);
    }

    public void DisplayBottomBar()
    {
        bottomBar.gameObject.SetActive(true);
    }
    public void DisplayBoosterBar()
    {
        boosterBar.gameObject.SetActive(true);
    }
    public void HideAllBar()
    {
        topBar.gameObject.SetActive(false);
        bottomBar.gameObject.SetActive(false);
        boosterBar.gameObject.SetActive(false);
    }

    public void HideSwipeCam()
    {
        swipeCamera.gameObject.SetActive(false);
    }
    
    private void StartTimer(float seconds)
    {
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

    public void SetFillProgressGame(float current, float total)
    {
        fillProgressBar.fillAmount = current/ total;
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
        this.PostEvent(EventID.ON_FROZE_BOOSTER_ENDED);
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