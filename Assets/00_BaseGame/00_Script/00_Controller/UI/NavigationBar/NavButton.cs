using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NavButton : MonoBehaviour
{
    public ENavType navType;
    [Space(5)]
    [SerializeField] private Button btnMain;
    [SerializeField] private TextMeshProUGUI txt;
    [SerializeField] private Image icon;
    [SerializeField] private Image background;
    [SerializeField] private Sprite originalBackground;
    
    private float originalYPosition;
    private float targetYPosition;
    private float originalScale;
    private float targetScale;
    private Vector2 originalBackgroundSize;
    private Vector2 targetBackgroundSize;
    private Sprite backgroundSelected;
    public void Init(float inOriginalYPosition, float inTargetYPosition, float inOriginalScale, float inTargetScale,
        Vector2 inOriginalBackgroundSize, Vector2 inTargetBackgroundSize, Sprite inBackgroundSelected)
    {
        this.originalYPosition = inOriginalYPosition;
        this.targetYPosition = inTargetYPosition;
        this.originalScale = inOriginalScale;
        this.targetScale = inTargetScale;
        this.originalBackgroundSize = inOriginalBackgroundSize;
        this.targetBackgroundSize = inTargetBackgroundSize;
        this.backgroundSelected = inBackgroundSelected;
    }
    
    public void HandleOnClicked(System.Action callback = null)
    {
        btnMain.onClick.AddListener(delegate
        {
            callback?.Invoke();
        });
    }
    public void HandleIconActive()
    {
        icon.transform.localPosition = new Vector3(0,targetYPosition,0);
        icon.transform.localScale = Vector3.one * targetScale; 
        background.sprite = backgroundSelected;
        background.rectTransform.sizeDelta = targetBackgroundSize;
        txt.gameObject.SetActive(true);
    }

    public void HandleIconInactive()
    {
        icon.transform.localPosition = new Vector3(0,originalYPosition,0);
        icon.transform.localScale = Vector3.one * originalScale;
        background.sprite = originalBackground;
        background.rectTransform.sizeDelta = originalBackgroundSize;
        txt.gameObject.SetActive(false);
    }

    //Odin
    public void InitSetup()
    {
        btnMain = GetComponent<Button>();
        background = GetComponent<Image>();
        icon = transform.Find("icon").GetComponent<Image>();
        txt = icon.transform.Find("txt").GetComponent<TextMeshProUGUI>();
    }
    
}