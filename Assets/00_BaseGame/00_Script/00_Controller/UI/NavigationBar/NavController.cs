using System.Collections.Generic;
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

    public void Init()
    {
        HomeBox.Setup().Show();
        foreach (var btn in this.lsButtons)
        {
            btn.Init(originalYPosition, targetYPosition, originalScale, targetScale, originalBackgroundSize, targetBackgroundSize,backgroundSelected);
            
            btn.HandleOnClicked(delegate
            {
                OnButtonSelected(btn);
            });
        }
    }
    
    private void OnButtonSelected(NavButton clickedButton)
    {
        if (clickedButton == currentNavButton)
        {
            return;
        }
        prevNavButton = currentNavButton;      
        currentNavButton = clickedButton;      
        if (prevNavButton != null)
        {
            prevNavButton.HandleIconInactive();
        }
        if (currentNavButton != null)
        {
            currentNavButton.HandleIconActive();
        }
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