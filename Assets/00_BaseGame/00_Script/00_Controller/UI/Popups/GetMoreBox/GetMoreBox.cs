using EventDispatcher;
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
    private GameController gameController;
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
        gameController.dataContains.giftData.Claim(currentBoosterType, 1);
        
        if (gameController.curSceneName.Equals(SceneName.GAME_PLAY))
        {
            GamePlayController.Instance.playerContains.boosterController.UpdateAmountBooster(currentBoosterType);
            this.PostEvent(EventID.ON_BOOSTER_CONDITION_CHANGED);
        }
        else if (gameController.curSceneName.Equals(SceneName.HOME_SCENE))
        {
            
        }
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
        gameController = GameController.Instance;
        dataBooster = gameController.dataContains.dataBooster;
    }

    private void UpdatePopupUI()
    {
        var typeSelected = dataBooster.boosterTypeSeleced;
        if (!IsNewBoosterSelected(typeSelected)) return;
        var boosterConflict = dataBooster.GetBoosterConflict(currentBoosterType);
        var sprIcon = boosterConflict.GetIcon();
        var priceInformation = boosterConflict.GetPrice();
        var descriptionInformation = boosterConflict.GetDescription();

        txtPrice.text = priceInformation.ToString();
        txtDescription.text = descriptionInformation;
        imgIcon.sprite = sprIcon;
        UIImageUtils.FitToTargetHeight(imgIcon,200);
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