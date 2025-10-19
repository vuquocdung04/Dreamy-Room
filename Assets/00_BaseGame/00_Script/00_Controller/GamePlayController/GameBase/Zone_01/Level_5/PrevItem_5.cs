

using DG.Tweening;

public class PrevItem_5 : PreGameItem
{
    
    Level_5 currentLevel;
    public override void Init()
    {
        currentLevel = (Level_5)GamePlayController.Instance.levelController.currentLevel;
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