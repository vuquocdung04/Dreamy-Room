using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardItem : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private Image imageBtn;
    [SerializeField] private TextMeshProUGUI txt;
    [SerializeField] private Transform fill;
    [SerializeField] private Transform iconAds;

    public void Init(System.Action callback = null)
    {
        btn.onClick.AddListener(delegate { callback?.Invoke(); });
    }

    public void InActiveBtn()
    {
        btn.enabled = false;
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
        txt.text = "Claim";
    }

    public void SetAsClaimed()
    {
        btn.enabled = false;
        iconAds.gameObject.SetActive(false);
        txt.text = "Claimed";
    }

    public void SetupOdin()
    {
        btn = transform.Find("Button").GetComponent<Button>();
        imageBtn = transform.Find("Button").GetComponent<Image>();
        txt = btn.transform.Find("txt").GetComponent<TextMeshProUGUI>();
        iconAds = btn.transform.Find("iconAds").GetComponent<Transform>();
        fill = transform.Find("fill");
    }
 }