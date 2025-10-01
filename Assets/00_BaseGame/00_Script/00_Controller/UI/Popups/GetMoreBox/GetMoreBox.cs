using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetMoreBox : BoxSingleton<GetMoreBox>
{
    public static GetMoreBox Setup()
    {
        return Path(PathPrefabs.GET_MORE_BOX);
    }

    public Button btnClose;
    public Button btnCloseByPanel;
    public Button btnBuyByCoin;
    public Button btnBuyByAds;
    public TextMeshProUGUI txtDescription;
    [Header("Icon Information"), Space(5)] public Image imgIcon;
    public TextMeshProUGUI txtPrice;

    private GiftType currentBoosterType;
    private DataBoosterBase dataBooster;
    protected override void Init()
    {
        currentBoosterType = GiftType.None;
        ActionClick(btnClose);
        ActionClick(btnCloseByPanel);
        ActionClick(btnBuyByCoin, IncreaseAmountBooster);
        ActionClick(btnBuyByAds, IncreaseAmountBooster);
        
        CacheDataBoosterReference();
    }

    protected override void InitState()
    {
        UpdatePopupUI();
    }

    private void IncreaseAmountBooster()
    {
        switch (currentBoosterType)
        {
            case GiftType.BoosterHint:
                UseProfile.Booster_Hint++;
                break;
            case GiftType.BoosterMagicWand:
                UseProfile.Booster_MagicWand++;
                break;
            case GiftType.BoosterFrozenTime:
                UseProfile.Booster_FrozeTime++;
                break;
        }

        Debug.LogError("IncreaseBooster");
        GamePlayController.Instance.playerContains.boosterController.UpdateAmountBooster(currentBoosterType);
    }
    private void ActionClick(Button btn, System.Action callback = null)
    {
        btn.onClick.AddListener(delegate
        {
            Close();
            GamePlayController.Instance.ResumeGame();
            callback?.Invoke();
        });
    }
    
    private void CacheDataBoosterReference()
    {
        dataBooster = GameController.Instance.dataContains.dataBooster;
    }
    private void UpdatePopupUI()
    {
        var typeSelected = dataBooster.boosterTypeSeleced;
        if(!IsNewBoosterSelected(typeSelected)) return;
        var boosterConflict = dataBooster.GetBoosterConflict(currentBoosterType);
        var sprIcon = boosterConflict.GetIcon();
        var priceInformation = boosterConflict.GetPrice();
        var descriptionInformation = boosterConflict.GetDescription();

        txtPrice.text = priceInformation.ToString();
        txtDescription.text = descriptionInformation;
        imgIcon.sprite = sprIcon;
        imgIcon.SetNativeSize();
    }

    private bool IsNewBoosterSelected(GiftType newBoosterType)
    {
        if (currentBoosterType == newBoosterType)
        {
            return false;
        }
        currentBoosterType = newBoosterType;
        return true;
    }
}