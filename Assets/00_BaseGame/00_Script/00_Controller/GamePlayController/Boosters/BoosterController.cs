
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BoosterController : MonoBehaviour
{
    [SerializeField] private RectTransform rectBoosters;
    [SerializeField] private List<BoosterBase> lsBoosters;

    public void Init()
    {
        HandleStateRectBoosters();
        
        var dataBooster = GameController.Instance.dataContains.dataBooster;
        InActiveBtns();
        foreach (var booster in lsBoosters)
        {
            var boosterAmount = GetBoosterAmountByType(booster.GetBoosterType());
            var dataConflict = dataBooster.GetBoosterConflict(booster.GetBoosterType());
            booster.Init(boosterAmount, dataConflict);
        }

        foreach (var booster in lsBoosters)
        {
            booster.AddClickListener(item =>
            {
                item.HandleAction();
            });
        }
    }
    
    private int GetBoosterAmountByType(GiftType type)
    {
        return type switch
        {
            GiftType.BoosterHint => UseProfile.Booster_Hint,
            GiftType.BoosterFrozenTime => UseProfile.Booster_FrozeTime,
            GiftType.BoosterMagicWand => UseProfile.Booster_MagicWand,
            _ => 0
        };
    }

    private void HandleStateRectBoosters()
    {
        var maxLevel = UseProfile.MaxUnlockedLevel;
        var isUnlocked = maxLevel > 2;
        rectBoosters.gameObject.SetActive(isUnlocked);

    }

    private void InActiveBtns()
    {
        foreach (var booster in this.lsBoosters)
            booster.InActiveBtn();
    }

    private void ActiveBtns()
    {
        foreach (var booster in this.lsBoosters)
            booster.ActiveBtn();
    }

    [Button("Setup Booster")]
    public void SetupItem()
    {
        foreach (var booster in lsBoosters)
        {
            booster.SetupOdin();
        }
    }
}