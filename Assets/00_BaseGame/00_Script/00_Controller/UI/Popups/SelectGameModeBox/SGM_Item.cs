
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
    private DataPlayer playerData;

    public void Init()
    {
        playerData = GameController.Instance.dataContains.DataPlayer;
        curAmount = GetBoosterAmountFromProfile(type);
        isUsed = GetBoosterUsedState(type); 
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
            SetBoosterUsedState(type, true);
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
    private bool GetBoosterUsedState(GiftType giftType)
    {
        return giftType switch
        {
            GiftType.BoosterX2Star => playerData.isUsedX2Star,
            GiftType.BoosterTimeBuffer => playerData.isUsedTimeBuffer,
            GiftType.BoosterBoxBuffet => playerData.isUsedBoxBuffer,
            _ => false
        };
    }
    private void SetBoosterUsedState(GiftType giftType, bool state)
    {
        switch (giftType)
        {
            case GiftType.BoosterX2Star:
                playerData.isUsedX2Star = state;
                break;
            case GiftType.BoosterTimeBuffer:
                playerData.isUsedTimeBuffer = state;
                break;
            case GiftType.BoosterBoxBuffet:
                playerData.isUsedBoxBuffer = state;
                break;
        }
    }
}