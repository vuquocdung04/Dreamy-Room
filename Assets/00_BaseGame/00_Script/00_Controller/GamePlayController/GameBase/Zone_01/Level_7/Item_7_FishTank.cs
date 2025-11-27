using Spine.Unity;
using UnityEngine;

public class Item_7_FishTank : ItemBase, IPostPlacementAction
{
    public SkeletonAnimation  skeletonAnimation;
    
    public void HandlePostPlacementAction()
    {
        gameObject.SetActive(false);
        skeletonAnimation.gameObject.SetActive(true);
        skeletonAnimation.AnimationState.SetAnimation(0, "animation", true);
    }
}