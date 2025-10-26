using EventDispatcher;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BoosterBase : MonoBehaviour
{
    #region Variables
    [SerializeField] protected GiftType boosterType;
    [SerializeField] protected TextMeshProUGUI txtBoosterAmount;
    [SerializeField] protected RectTransform transLockedState;
    [SerializeField] protected RectTransform transUnlockedState;

    [SerializeField] protected RectTransform transAmountNotEmpty;
    [SerializeField] protected RectTransform transAmountEmpty;
    [SerializeField] protected Button btn;
    [SerializeField] protected int boosterAmount;
    private BoosterConflict cachedDataConflict;
    private int cachedMaxLevel;
    #endregion
    
    public GiftType GetBoosterType() => boosterType;

    #region Initialization
    public virtual void Init(int curBoosterAmount)
    {
        cachedMaxLevel = UseProfile.MaxUnlockedLevel;
        IncreaseAmount(curBoosterAmount);
        
        var levelUnlock = cachedDataConflict.GetLevelUnlock();
        
        var isUnlocked = cachedMaxLevel >= levelUnlock;
        transLockedState.gameObject.SetActive(!isUnlocked);
        transUnlockedState.gameObject.SetActive(isUnlocked);

        if (!isUnlocked) return;
        UpdateAmountUI();
        UpdateBoosterButtonState();
        this.RegisterListener(EventID.ON_BOOSTER_CONDITION_CHANGED, UpdateBoosterButtonState);
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.ON_BOOSTER_CONDITION_CHANGED, UpdateBoosterButtonState);
    }
    #endregion

    #region Amount Management
    public void IncreaseAmount(int amount)
    {
        boosterAmount = amount;
    }

    public void UpdateAmountUI()
    {
        bool isBoosterAvailable = boosterAmount > 0;
        transAmountNotEmpty.gameObject.SetActive(isBoosterAvailable);
        transAmountEmpty.gameObject.SetActive(!isBoosterAvailable);
        
        if (isBoosterAvailable)
            txtBoosterAmount.text = boosterAmount.ToString();
    }
    #endregion

    #region UI & Interaction
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
    #endregion

    #region Booster Logic
    public void HandleAction()
    {
        if (!IsUnlocked()) return;
        GameController.Instance.dataContains.dataBooster.boosterTypeSeleced = boosterType;
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

    private bool IsUnlocked() => cachedMaxLevel >= cachedDataConflict.GetLevelUnlock();
    #endregion

    #region Setup
    // Setup
    public void SetupOdin()
    {
        transLockedState = transform.Find("Lock").GetComponent<RectTransform>();
        transUnlockedState = transform.Find("UnLock").GetComponent<RectTransform>();
        transAmountNotEmpty = transUnlockedState.Find("imgAmount").GetComponent<RectTransform>();
        transAmountEmpty = transUnlockedState.Find("imgPlus").GetComponent<RectTransform>();
        txtBoosterAmount = transAmountNotEmpty.Find("txt").GetComponent<TextMeshProUGUI>();
        btn = GetComponent<Button>();
    }
    #endregion

    #region Abstract Methods
    protected abstract void OnBoosterUsed();
    protected abstract void UpdateUseProfileAmount(int amount);
    #endregion
}