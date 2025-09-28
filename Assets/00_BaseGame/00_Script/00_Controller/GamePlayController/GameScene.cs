
using DG.Tweening;
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
    
    
    private Camera mainCamera;
    private readonly float defaultOrthographicSize = 10f;
    private readonly float zoomedOrthographicSize = 8f;
    private bool isZoomed;
    public void Init()
    {
        btnPause.onClick.AddListener(delegate
        {
            SettingGameBox.Setup().Show();
        });
        btnRemoveAds.onClick.AddListener(delegate
        {
            RemoveAdsBox.Setup().Show();
        });
        
        btnZoom.onClick.AddListener(ToggleZoomInOut);
    }

    private void ToggleZoomInOut()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        
        float targetSize = isZoomed ? defaultOrthographicSize : zoomedOrthographicSize;
        imgZoom.sprite = isZoomed ? sprZoomIn : sprZoomOut;
        
        mainCamera.DOOrthoSize(targetSize, 0.2f);
        isZoomed = !isZoomed;
    }
    
    
}
