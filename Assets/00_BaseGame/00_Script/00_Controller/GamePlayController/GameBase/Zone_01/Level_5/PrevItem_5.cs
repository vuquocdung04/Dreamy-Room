

using DG.Tweening;
using UnityEngine;

public class PrevItem_5 : PreGameItem
{
    private SpriteRenderer spr;
    private Level_5 currentLevel;
    public override void Init()
    {
        currentLevel = (Level_5)GamePlayController.Instance.levelController.currentLevel;
        Debug.LogError(currentLevel.gameObject.name);
        Target = currentLevel.targetPlacedPrevItem;
        base.Init();
        
    }
    protected override void OnDoneSnap()
    {
        currentLevel.HandlePrevItem();
        coll2D.enabled = false;
        transform.DOMove(Target.transform.position, 0.2f).SetEase(Ease.Linear).SetLink(gameObject).OnComplete(delegate
        {
            Destroy(gameObject);
        });
    }
}