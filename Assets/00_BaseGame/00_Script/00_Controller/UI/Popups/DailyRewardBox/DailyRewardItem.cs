
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardItem : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private Image imageBtn;
    [SerializeField] private Transform fill;
    [SerializeField] private Transform iconAds;
    [SerializeField] private LocalizedText lcBtn;
    
    public void AddClickListener(System.Action callback = null)
    {
        btn.onClick.AddListener(delegate { callback?.Invoke(); });
    }
    public void UpdateLocalization(string localizationKey)
    {
        if (lcBtn == null) return;
        lcBtn.Init(localizationKey);
    }
    
    public void UpdateImageBtn(Sprite btnSprite)
    {
        imageBtn.sprite = btnSprite;
    }

    public void SetAsClaimable()
    {
        btn.enabled = true;
        fill.gameObject.SetActive(true);
        iconAds.gameObject.SetActive(true);
    }

    public void SetAsClaimed()
    {
        btn.enabled = false;
        iconAds.gameObject.SetActive(false);
    }
    public void SetAsFree()
    {
        btn.enabled = false;
        fill.gameObject.SetActive(false);
        iconAds.gameObject.SetActive(false);
    }
    public void SetupOdin()
    {
        btn = transform.Find("Button").GetComponent<Button>();
        imageBtn = transform.Find("Button").GetComponent<Image>();
        lcBtn = btn.transform.Find("txt").GetComponent<LocalizedText>();
        iconAds = btn.transform.Find("iconAds").GetComponent<Transform>();
        fill = transform.Find("fill");
    }
 }