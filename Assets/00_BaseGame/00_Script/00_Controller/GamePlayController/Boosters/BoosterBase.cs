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
    
    public void InActiveBtn()
    {
        if(!IsUnlocked()) return;
        btn.interactable = false;
    }

    public void ActiveBtn()
    {
        if(!IsUnlocked()) return;
        btn.interactable = true;
    }

    public void HandleAction()
    {
        if(!IsUnlocked()) return;
        
        if(boosterAmount <= 0)
            GetMoreBox.Setup().Show();
        
        boosterAmount--;
        UpdateUseProfileAmount(boosterAmount);
        UpdateAmountUI(IsBoosterAvailable());
        OnBoosterUsed();
    }
    
    public void Init(int curBoosterAmount)
    {
        cachedMaxLevel = UseProfile.MaxUnlockedLevel;
        boosterAmount = curBoosterAmount;
        
        var levelUnlock = cachedDataConflict.GetLevelUnlock();
        
        var isUnlocked = cachedMaxLevel >= levelUnlock;
        transLockedState.gameObject.SetActive(!isUnlocked);
        transUnlockedState.gameObject.SetActive(isUnlocked);

        if (!isUnlocked) return;
        
        UpdateAmountUI(IsBoosterAvailable());
    }

    private bool IsBoosterAvailable()
    {
        return boosterAmount > 0;
    }

    private void UpdateAmountUI(bool hasItems)
    {
        transAmountNotEmpty.gameObject.SetActive(hasItems);
        transAmountEmpty.gameObject.SetActive(!hasItems);

        if (hasItems)
            txtBoosterAmount.text = boosterAmount.ToString();
    }

    private bool IsUnlocked() => cachedMaxLevel >= cachedDataConflict.GetLevelUnlock();
       
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