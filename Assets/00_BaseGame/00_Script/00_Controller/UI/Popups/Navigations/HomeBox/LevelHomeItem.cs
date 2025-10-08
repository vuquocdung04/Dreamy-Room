using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelHomeItem : MonoBehaviour
{
    [SerializeField] private ItemHomeType type;
    [SerializeField] private int idLevel;
    [SerializeField] private Button btn;
    [SerializeField] private Image icon;
    [SerializeField] private Image imgBg;
    [SerializeField] private TextMeshProUGUI txtLevel;
    [SerializeField] private Image imgHighlight;

    public int GetId() => idLevel;
    public void SetId(int id) => idLevel = id;

    public ItemHomeType GetItemHomeType() => type;
    public void SetItemHomeType(ItemHomeType typeParam) => this.type = typeParam;

    public void AddClickListener(System.Action<LevelHomeItem> callback = null)
    {
        btn.onClick.AddListener(delegate { callback?.Invoke(this); });
    }

    public void InitInfo(Sprite spIcon)
    {
        icon.sprite = spIcon;
        txtLevel.text = idLevel.ToString();
    }

    public void FitIconToTargetHeight(float targetHeight)
    {
        if(icon.sprite == null) return;
    
        Rect rect = icon.sprite.rect;
        
        float aspectRatio = rect.width / rect.height;
        
        float newWidth = targetHeight * aspectRatio;
        icon.rectTransform.sizeDelta = new Vector2(newWidth, targetHeight);
    }
    
    public void UpdateState(Color colorIcon, Sprite sprBg, bool isLock = false)
    {
        switch (isLock)
        {
            case false:
                icon.color = Color.white;
                break;
            case true:
                imgBg.sprite = sprBg;
                icon.color = colorIcon;
                break;
        }
    }

    public void ActiveHighlight()
    {
        imgHighlight.gameObject.SetActive(true);
    }

    //Odin
    public void SetupOdin()
    {
        imgBg = GetComponent<Image>();
        icon = transform.Find("icon").GetComponent<Image>();
        txtLevel = transform.Find("txt").GetComponent<TextMeshProUGUI>();
        btn = GetComponent<Button>();
        imgHighlight = transform.Find("imgSelected").GetComponent<Image>();
    }
}