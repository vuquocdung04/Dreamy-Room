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
        btnClose.onClick.AddListener(Close);
        btnCloseByPanel.onClick.AddListener(Close);
        btnBuyByCoin.onClick.AddListener(delegate { });

        btnBuyByAds.onClick.AddListener(delegate { });
        
        CacheDataBoosterReference();
    }

    protected override void InitState()
    {
        UpdatePopupUI();
    }

    private void CacheDataBoosterReference()
    {
        dataBooster = GameController.Instance.dataContains.dataBooster;
    }
    private void UpdatePopupUI()
    {
        var typeSelected = dataBooster.boosterTypeSeleced;
        
        if(!IsNewBoosterSelected(typeSelected)) return;

        var sprIcon = dataBooster.GetSpriteByType(typeSelected);
        var priceInformation = dataBooster.GetPriceByType(typeSelected);
        var descriptionInformation = dataBooster.GetDescriptionByType(typeSelected);

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
        return false;
    }
}