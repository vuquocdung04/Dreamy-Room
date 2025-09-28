
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BoosterController : MonoBehaviour
{
    [SerializeField] private List<BoosterBase> lsBoosters;

    public void Init()
    {
        var dataBooster = GameController.Instance.dataContains.dataBooster;
        foreach (var booster in lsBoosters)
        {
            var boosterAmount = GetBoosterAmountByType(booster.GetBoosterType());
            var dataConflict = dataBooster.GetBoosterConflict(booster.GetBoosterType());
            booster.UpdateStateUI(boosterAmount, dataConflict);
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

    [Button("Setup Booster")]
    public void SetupItem()
    {
        foreach (var booster in lsBoosters)
        {
            booster.SetupOdin();
        }
    }
}