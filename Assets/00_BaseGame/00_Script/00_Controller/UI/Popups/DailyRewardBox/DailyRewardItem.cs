
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardItem : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private Image imageBtn;
    [SerializeField] private Transform fill;
    [SerializeField] private Transform iconAds;
    [SerializeField] private LocalizedText lcBtn;
    private ItemState currentState = ItemState.Free;

    private enum ItemState
    {
        Free,      // Chưa tới lượt
        Claimable, // Có thể claim
        Claimed    // Đã claim
    }
    public void AddClickListener(System.Action callback = null)
    {
        btn.onClick.AddListener(delegate { callback?.Invoke(); });
    }

    public void InitLocalization()
    {
        lcBtn.Init();
        UpdateTextByState();
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
        currentState = ItemState.Claimable;
        UpdateTextByState();
    }

    public void SetAsClaimed()
    {
        btn.enabled = false;
        iconAds.gameObject.SetActive(false);
        currentState = ItemState.Claimed;
        UpdateTextByState();
    }
    public void SetAsFree()
    {
        btn.enabled = false;
        fill.gameObject.SetActive(false);
        iconAds.gameObject.SetActive(false);
        currentState = ItemState.Free;
        UpdateTextByState();
    }

    private void UpdateTextByState()
    {
        if (lcBtn == null) return;

        var dataDaily = GameController.Instance.dataContains.dataDaily;

        switch (currentState)
        {
            case ItemState.Claimable:
                lcBtn.Init(dataDaily.KeyClaim);
                break;
            case ItemState.Claimed:
                lcBtn.Init(dataDaily.KeyClaimed);
                break;
            case ItemState.Free:
                lcBtn.Init(dataDaily.KeyFree);
                break;
        }
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