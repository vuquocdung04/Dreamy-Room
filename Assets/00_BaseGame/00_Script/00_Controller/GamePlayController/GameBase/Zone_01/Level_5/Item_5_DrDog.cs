using Spine.Unity;
public class Item_5_DrDog : ItemBase, IPostPlacementAction
{
    public SkeletonAnimation skeletonAnimation;

    public void HandlePostPlacementAction()
    {
        gameObject.SetActive(false);
        skeletonAnimation.gameObject.SetActive(true);
        skeletonAnimation.AnimationState.SetAnimation(0, "idle2", true);
    }
}