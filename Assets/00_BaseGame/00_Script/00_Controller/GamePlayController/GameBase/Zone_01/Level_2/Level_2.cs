
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Level_2 : LevelBase
{
    public Transform duckTrans;
    protected override ItemSlot CreateItemSlotInstance(GameObject go)
    {
        return go.AddComponent<Slot_2>();
    }
    

    protected override async UniTask PreWinGameLogic()
    {
        await base.PreWinGameLogic();
        var duckSpr =  duckTrans.GetComponent<SpriteRenderer>();
        duckTrans.localScale = Vector3.zero;
        duckTrans.gameObject.SetActive(true);
        var seq = DOTween.Sequence();
        seq.Append(duckTrans.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutElastic));
        seq.Append(duckTrans.DOLocalMoveY(-3.37f, 0.5f).SetEase(Ease.OutBack));
        seq.AppendCallback(() => duckSpr.sortingLayerName = SortingLayerName.DEFAULT);
        seq.Append(duckTrans.DOLocalJump(new Vector3(-0.74f, -0.41f), 2f, 1, 1.25f).SetEase(Ease.Linear));
        await seq.AsyncWaitForCompletion();
    }
}