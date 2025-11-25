using Spine.Unity;
using UnityEngine;

public class Item_1_ThuBong : ItemBase, IPostPlacementAction
{
    public SkeletonAnimation skeletonAnimation;
    public void HandlePostPlacementAction()
    {
        gameObject.SetActive(false);
        skeletonAnimation.gameObject.SetActive(true);
        var entryTrack = skeletonAnimation.AnimationState.SetAnimation(0, "1-start-place", false);
        entryTrack.Complete += t =>
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "4-complete-loop", true);
        };
    }
}