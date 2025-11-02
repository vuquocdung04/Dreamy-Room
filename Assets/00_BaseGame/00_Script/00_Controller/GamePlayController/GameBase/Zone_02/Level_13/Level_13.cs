using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Level_13 : LevelBase
{
    public Transform holderMoon;
    public SpriteRenderer rightCloud;
    public SpriteRenderer leftCloud;

    public override void Init()
    {
        base.Init();
        holderMoon.gameObject.SetActive(false);
    }

    protected override ItemSlot CreateItemSlotInstance(GameObject go)
    {
        return go.AddComponent<Slot_13>();
    }

    protected override async UniTask OnBeforeWinCompleted()
    {
        await base.OnBeforeWinCompleted();
    
        holderMoon.gameObject.SetActive(true);
        var posR = rightCloud.transform.localPosition;
        var posL = leftCloud.transform.localPosition;
        var seq = DOTween.Sequence();
        var s1 = seq.Append(rightCloud.transform.DOLocalMoveX(posR.x + 2f, 1f));
        var s2 = seq.Join(rightCloud.DOFade(0f, 1f));
        var s3 = seq.Join(leftCloud.transform.DOLocalMoveX(posL.x - 2f, 1f));
        var s4 = seq.Join(leftCloud.DOFade(0f, 1f));
        await seq.ToUniTask();
    }
}