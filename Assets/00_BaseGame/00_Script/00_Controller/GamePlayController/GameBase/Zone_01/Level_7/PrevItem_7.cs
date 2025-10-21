using DG.Tweening;
using UnityEngine;

public class PrevItem_7 : PreGameItem
{
    private Level_7 levelCtrl;

    public override void Init()
    {
        levelCtrl = (Level_7)GamePlayController.Instance.levelController.currentLevel;
        Target = levelCtrl.bin;
        base.Init();
    }

    protected override void OnDoneSnap()
    {
        levelCtrl.HandlePrevItem();
        coll2D.enabled = false;
        var binTrans = levelCtrl.bin.position;
        var randX = Random.Range(-0.1f, 0.1f);
        var randY = Random.Range(-0.1f, 0.1f);
        var targetMove = binTrans + new Vector3(randX, randY);
        transform.DOLocalMove(targetMove,0.2f).SetEase(Ease.Linear);
    }
}