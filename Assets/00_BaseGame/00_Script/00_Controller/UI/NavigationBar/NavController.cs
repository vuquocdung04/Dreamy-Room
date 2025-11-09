using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class NavController : MonoBehaviour
{
    [SerializeField] private float originalYPosition;
    [SerializeField] private float targetYPosition;
    [SerializeField] private float originalScale;
    [SerializeField] private float targetScale;
    [SerializeField] private Vector2 originalBackgroundSize;
    [SerializeField] private Vector2 targetBackgroundSize;
    [SerializeField] private Sprite backgroundSelected;
    [Header("Debug"),Space(6)]
    [SerializeField] private NavButton currentNavButton;
    [SerializeField] private NavButton prevNavButton;
    [SerializeField] bool isBusy;
    public List<NavButton> lsButtons;

    public void Init()
    {
        GetBoxInstance(ENavType.Home).Show();
        
        foreach (var btn in this.lsButtons)
        {
            btn.Init(originalYPosition, targetYPosition, originalScale, targetScale, originalBackgroundSize, targetBackgroundSize, backgroundSelected);
            
            if (btn.navType == ENavType.Home)
            {
                currentNavButton = btn;
                currentNavButton.HandleIconActive();
            }

            btn.HandleOnClicked(() => OnNavButtonClicked(btn));
        }
    }

    private void OnNavButtonClicked(NavButton clickedButton)
    {
        if (isBusy || clickedButton == currentNavButton) return;
        
        OnButtonSelected(clickedButton);
    }

    public void GoToScreen(ENavType targetType)
    {
        NavButton targetButton = lsButtons.FirstOrDefault(btn => btn.navType == targetType);
        if (targetButton != null && targetButton != currentNavButton)
        {
            OnNavButtonClicked(targetButton);
        }
    }

    private void OnButtonSelected(NavButton clickedButton)
    {
        isBusy = true;
        GameController.Instance.audioController.PlaySfx(AudioKeyType.UIClick);
        HandleScreenTransition(clickedButton);
        UpdateNavButtonStates(clickedButton);
    }

    /// <summary>
    /// Xử lý hiệu ứng chuyển đổi giữa các màn hình (Box).
    /// </summary>
    private void HandleScreenTransition(NavButton clickedButton)
    {
        int prevIndex = lsButtons.IndexOf(currentNavButton);
        int currentIndex = lsButtons.IndexOf(clickedButton);
        
        BaseBox prevBox = GetBoxInstance(currentNavButton.navType);
        BaseBox currentBox = GetBoxInstance(clickedButton.navType);

        bool slideToLeft = currentIndex < prevIndex;

        if (prevBox != null)
        {
            // Nếu box mới trượt từ phải sang, box cũ trượt sang trái (slideOutToLeft = true)
            // Nếu box mới trượt từ trái sang, box cũ trượt sang phải (slideOutToLeft = false)
            prevBox.CloseSliding(slideOutToLeft: !slideToLeft);
        }

        Tween showTween = null;
        if (currentBox != null)
        {
            // slideInFromLeft = true nếu box mới ở bên trái box cũ
            showTween = currentBox.ShowSliding(slideInFromLeft: slideToLeft);
        }

        // Mở khóa isBusy sau khi animation hoàn tất
        HandleAnimationCompletion(showTween);
    }

    /// <summary>
    /// Đặt callback để set isBusy = false khi animation hoàn thành.
    /// </summary>
    private void HandleAnimationCompletion(Tween tween)
    {
        if (tween != null)
        {
            tween.OnComplete(() => isBusy = false);
        }
        else
        {
            isBusy = false; // Mở khóa ngay lập tức nếu không có animation
        }
    }

    /// <summary>
    /// Cập nhật trạng thái active/inactive cho các button điều hướng.
    /// </summary>
    private void UpdateNavButtonStates(NavButton clickedButton)
    {
        prevNavButton = currentNavButton;
        currentNavButton = clickedButton;

        if (prevNavButton != null)
            prevNavButton.HandleIconInactive();

        if (currentNavButton != null)
            currentNavButton.HandleIconActive();
    }
    
    private BaseBox GetBoxInstance(ENavType type)
    {
        BaseBox newBox = null;
        switch (type)
        {
            case ENavType.Home: newBox = HomeBox.Setup(); break;
            case ENavType.Shop: newBox = ShopBox.Setup(); break;
            case ENavType.Rank: newBox = RankBox.Setup(); break;
            case ENavType.Team: newBox = TeamBox.Setup(); break;
            case ENavType.Collection: newBox = CollectionBox.Setup(); break;
        }
        
        return newBox;
    }

    [Button("Setup Btn", ButtonSizes.Large)]
    void SetupBtn()
    {
        foreach (var btn in this.lsButtons)
        {
            btn.InitSetup();
        }
    }
}