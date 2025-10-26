
using EventDispatcher;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SGM_Item : MonoBehaviour
{
    public GiftType type;
    public RectTransform rectUsed;
    public RectTransform info;
    public TextMeshProUGUI txtAmount;
    public RectTransform iconPlus;
    public Button btn;
    private int curAmount;
    private bool isUsed;

    public void Init()
    {
        curAmount = GetBoosterAmountFromProfile(type);
        UpdateUI();
        btn.onClick.AddListener(HandleAction);
        this.RegisterListener(EventID.HOME_BOOSTER_CHANGED, BoughtItem);
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.HOME_BOOSTER_CHANGED, BoughtItem);
    }

    private void BoughtItem(object obj = null)
    {
        HandleAmountFromProfile(1);
        UpdateUI();
    }

    private void HandleAmountFromProfile(int amount)
    {
        GameController.Instance.dataContains.giftData.Claim(type, amount);
        curAmount += amount;
    }
    
    private void HandleAction()
    {
        if(isUsed) return;
        GameController.Instance.dataContains.dataBooster.boosterTypeSeleced = type;
        if (curAmount > 0)
        {
            isUsed = true;
            HandleAmountFromProfile(-1);
            UpdateUI();
        }
        else
        {
            GetMoreBox.Setup().Show();
        }
    }

    private void UpdateUI()
    {
        bool isAvailable = curAmount > 0;
        rectUsed.gameObject.SetActive(isUsed);
        info.gameObject.SetActive(!isUsed);
        btn.enabled = !isUsed;

        if (!isUsed)
        {
            txtAmount.gameObject.SetActive(isAvailable);
            iconPlus.gameObject.SetActive(!isAvailable);
            if(isAvailable)
                txtAmount.text = curAmount.ToString();
        }
    }
    private int GetBoosterAmountFromProfile(GiftType giftType)
    {
        return giftType switch
        {
            GiftType.BoosterX2Star => UseProfile.Booster_X2Star,
            GiftType.BoosterTimeBuffer => UseProfile.Booster_TimeBuffer,
            GiftType.BoosterBoxBuffet => UseProfile.Booster_BoxBuffer,
            _ => 0
        };
    }
}