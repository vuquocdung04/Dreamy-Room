
using EventDispatcher;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BoosterBase : MonoBehaviour
{
    [SerializeField] protected GiftType boosterType;
    public GiftType GetBoosterType() => boosterType;
    [SerializeField] protected TextMeshProUGUI txtBoosterAmount;
    [SerializeField] protected RectTransform transLockedState;
    [SerializeField] protected RectTransform transUnlockedState;

    [SerializeField] protected RectTransform transAmountNotEmpty;
    [SerializeField] protected RectTransform transAmountEmpty;
    [SerializeField] protected Button btn;
    [SerializeField] protected int boosterAmount;
    private BoosterConflict cachedDataConflict;
    private int cachedMaxLevel;
    protected abstract void OnBoosterUsed();
    protected abstract void UpdateUseProfileAmount(int amount);
    
        
    public void Init(int curBoosterAmount)
    {
        cachedMaxLevel = UseProfile.MaxUnlockedLevel;
        IncreaseAmount(curBoosterAmount);
        
        var levelUnlock = cachedDataConflict.GetLevelUnlock();
        
        var isUnlocked = cachedMaxLevel >= levelUnlock;
        transLockedState.gameObject.SetActive(!isUnlocked);
        transUnlockedState.gameObject.SetActive(isUnlocked);

        if (!isUnlocked) return;
        this.RegisterListener(EventID.ON_BOOSTER_CONDITION_CHANGED, UpdateBoosterButtonState);
        UpdateAmountUI();
        this.PostEvent(EventID.ON_BOOSTER_CONDITION_CHANGED);
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.ON_BOOSTER_CONDITION_CHANGED, UpdateBoosterButtonState);
    }

    public void IncreaseAmount(int amount)
    {
        boosterAmount = amount;
    }
    
    public void AddClickListener(System.Action<BoosterBase> callback = null)
    {
        btn.onClick.AddListener(delegate
        {
            callback?.Invoke(this);
        });
    }

    public void SetBoosterConflict(BoosterConflict conflict)
    {
        cachedDataConflict = conflict;
    }
    
    public void HandleAction()
    {
        if(!IsUnlocked()) return;
        GameController.Instance.dataContains.dataBooster.boosterTypeSeleced =  boosterType;
        if (boosterAmount <= 0)
        {
            GamePlayController.Instance.PauseGame();
            GetMoreBox.Setup().Show();
            return;
        }
        
        boosterAmount--;
        UpdateUseProfileAmount(boosterAmount);
        UpdateAmountUI();
        OnBoosterUsed();
    }


    public void UpdateAmountUI()
    {
        bool isBoosterAvailable = boosterAmount > 0;
        transAmountNotEmpty.gameObject.SetActive(isBoosterAvailable);
        transAmountEmpty.gameObject.SetActive(!isBoosterAvailable);
        
        if (isBoosterAvailable)
            txtBoosterAmount.text = boosterAmount.ToString();
    }
    private void UpdateBoosterButtonState(object obj = null)
    {
        if (boosterAmount <= 0)
        {
            btn.interactable = true;
            return;
        }
        bool canUse = CheckBoosterSpecificConditions();
        btn.interactable = canUse;
    }

    private bool IsUnlocked() => cachedMaxLevel >= cachedDataConflict.GetLevelUnlock();


    private bool CheckBoosterSpecificConditions()
    {
        switch (boosterType)
        {
            case GiftType.BoosterHint:
                return GamePlayController.Instance.levelController.HasItemOutOfBox();
            case GiftType.BoosterMagicWand:
                return GamePlayController.Instance.levelController.HasReadyShadows();
            default:
                return true;
        }
    }
    //Setup
    public void SetupOdin()
    {
        transLockedState = transform.Find("Lock").GetComponent<RectTransform>();
        transUnlockedState = transform.Find("UnLock").GetComponent<RectTransform>();
        transAmountNotEmpty = transUnlockedState.Find("imgAmount").GetComponent<RectTransform>();
        transAmountEmpty = transUnlockedState.Find("imgPlus").GetComponent<RectTransform>();
        txtBoosterAmount = transAmountNotEmpty.Find("txt").GetComponent<TextMeshProUGUI>();
        btn = GetComponent<Button>();
    }
}