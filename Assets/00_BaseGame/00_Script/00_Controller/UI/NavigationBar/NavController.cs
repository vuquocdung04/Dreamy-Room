using System.Collections.Generic;
using System.Linq;
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
    
    public List<NavButton> lsButtons;
    private Dictionary<ENavType, BaseBox> boxInstances = new();
    public void Init()
    {
        var homeBox = GetBoxInstance(ENavType.Home);
        homeBox.Show();
        
        foreach (var btn in this.lsButtons)
        {
            btn.Init(originalYPosition, targetYPosition, originalScale, targetScale, originalBackgroundSize, targetBackgroundSize,backgroundSelected);
            
            if (btn.navType == ENavType.Home)
            {
                currentNavButton = btn;
                currentNavButton.HandleIconActive();
            }
            btn.HandleOnClicked(delegate
            {
                OnButtonSelected(btn);
            });
        }
    }

    
    // shortcut
    public void GoToScreen(ENavType targetType)
    {
        NavButton targetButton = lsButtons.FirstOrDefault(btn => btn.navType == targetType);
        if (targetButton != null && targetButton != currentNavButton)
        {
            OnButtonSelected(targetButton);

        }
    }

    private void OnButtonSelected(NavButton clickedButton)
    {
        if (clickedButton == currentNavButton)
            return;
        
        int prevIndex = lsButtons.IndexOf(currentNavButton);
        int currentIndex = lsButtons.IndexOf(clickedButton);
        
        BaseBox prevBox = GetBoxInstance(currentNavButton.navType);
        BaseBox currentBox = GetBoxInstance(clickedButton.navType);
        
        // --- Logic quyết định hướng di chuyển ---
        if (currentIndex > prevIndex) // Di chuyển sang phải
        {
            if (prevBox != null) prevBox.CloseSliding(slideOutToLeft: true);
            if (currentBox != null) currentBox.ShowSliding(slideInFromLeft: false);
        }
        else // Di chuyển sang trái
        {
            if (prevBox != null) prevBox.CloseSliding(slideOutToLeft: false);
            if (currentBox != null) currentBox.ShowSliding(slideInFromLeft: true);
        }
        
        // cap nhat trang thai button
        prevNavButton = currentNavButton;      
        currentNavButton = clickedButton;      
        if (prevNavButton != null)
            prevNavButton.HandleIconInactive();
        if (currentNavButton != null)
            currentNavButton.HandleIconActive();
    }

    private BaseBox GetBoxInstance(ENavType type)
    {
        if (boxInstances.ContainsKey(type) && boxInstances[type] != null)
        {
            return boxInstances[type];
        }

        BaseBox newBox = null;
        switch (type)
        {
            case ENavType.Home:
                newBox = HomeBox.Setup();
                break;
            case ENavType.Shop:
                newBox =  ShopBox.Setup();
                break;
            case ENavType.Rank:
                newBox =  RankBox.Setup();
                break;
            case ENavType.Team:
                newBox =  TeamBox.Setup();
                break;
            case ENavType.Collection:
                newBox =  CollectionBox.Setup();
                break;
        }

        if (newBox != null)
        {
            boxInstances[type] = newBox;
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