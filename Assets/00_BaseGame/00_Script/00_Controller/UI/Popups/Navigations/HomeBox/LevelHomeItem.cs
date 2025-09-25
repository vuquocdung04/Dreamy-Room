using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelHomeItem : MonoBehaviour
{
    [SerializeField] private int idLevel;
    [SerializeField] private Button btn;
    [SerializeField] private Image icon;
    [SerializeField] private Image imgBg;
    [SerializeField] private TextMeshProUGUI txtLevel;
    
    public int GetId() => idLevel;
    public void SetId(int  id) => idLevel = id;

    public void AddClickListener(System.Action<LevelHomeItem> callback = null)
    {
        btn.onClick.AddListener(delegate
        {
            callback?.Invoke(this);
        });
    }

    public void InitInfo(Sprite spIcon)
    {
        icon.sprite = spIcon;
        txtLevel.text = idLevel.ToString();
    }

    public void UpdateState(Color colorIcon, Color colorBg)
    {
        imgBg.color = colorIcon;
        icon.color = colorBg;
    }
    
    //Odin
    public void SetupOdin()
    {
        imgBg = GetComponent<Image>();
        icon = transform.Find("icon").GetComponent<Image>();
        txtLevel = transform.Find("txt").GetComponent<TextMeshProUGUI>();
        btn =GetComponent<Button>();
    }
}