
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
    
    private CameraController cameraController;
    public void Init()
    {
        cameraController = GamePlayController.Instance.playerContains.cameraController;
        
        btnPause.onClick.AddListener(delegate
        {
            SettingGameBox.Setup().Show();
        });
        btnRemoveAds.onClick.AddListener(delegate
        {
            RemoveAdsBox.Setup().Show();
        });
        
        btnZoom.onClick.AddListener(HandleToggleZoomInOut);
    }

    private void HandleToggleZoomInOut()
    {
        cameraController.ToggleZoomInOut();
        var isZoomed = cameraController.GetIsZoomed();
        imgZoom.sprite = isZoomed ? sprZoomOut : sprZoomIn;
    }
    
}
