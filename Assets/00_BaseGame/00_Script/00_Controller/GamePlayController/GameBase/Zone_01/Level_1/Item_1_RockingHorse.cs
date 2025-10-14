
using DG.Tweening;
using UnityEngine;

public class Item_1_RockingHorse : ItemBase, IPostPlacementAction
{
    private Tween placedTween;

    public void HandlePostPlacementAction()
    {
        placedTween = transform.DORotate(new Vector3(0, 0, 10f), 1f)
            .SetEase(Ease.InOutSine) 
            .SetLoops(-1, LoopType.Yoyo);
    }
    
    private void OnDestroy()
    {
        if (placedTween != null)
        {
            placedTween.Kill();
            placedTween = null;
        }
    }
}