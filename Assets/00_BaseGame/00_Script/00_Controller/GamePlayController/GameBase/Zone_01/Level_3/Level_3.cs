using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Level_3 : LevelBase
{
    public Transform character;
    protected override ItemSlot CreateItemSlotInstance(GameObject go)
    {
        return go.AddComponent<Slot_3>();
    }

    protected override async UniTask HandlePrevWinGame()
    {
        await base.HandlePrevWinGame();
        character.gameObject.SetActive(true);
        character.localScale = Vector3.zero;
        Tween charTween = character.DOScale(Vector3.one * 0.675f, 0.5f).SetEase(Ease.OutElastic);
        await charTween.ToUniTask();
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
    }
}