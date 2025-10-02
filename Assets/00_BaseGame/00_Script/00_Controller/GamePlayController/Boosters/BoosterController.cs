
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
        foreach(var booster in lsBoosters) booster.SetBoosterConflict(dataBooster.GetBoosterConflict(booster.GetBoosterType()));
        
        foreach (var booster in lsBoosters)
        {
            var boosterAmount = GetBoosterAmountByType(booster.GetBoosterType());
            booster.Init(boosterAmount);
        }

        foreach (var booster in lsBoosters)
        {
            booster.AddClickListener(item =>
            {
                item.HandleAction();
            });
        }
    }

    public void UpdateAmountBooster(GiftType type)
    {
        foreach(var booster in lsBoosters)
            if (booster.GetBoosterType() == type)
            {
                var boosterAmount = GetBoosterAmountByType(booster.GetBoosterType());
                booster.IncreaseAmount(boosterAmount);
                booster.UpdateAmountUI();
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
    
    [Button("Setup Booster")]
    public void SetupItem()
    {
        foreach (var booster in lsBoosters)
        {
            booster.SetupOdin();
        }
    }
}