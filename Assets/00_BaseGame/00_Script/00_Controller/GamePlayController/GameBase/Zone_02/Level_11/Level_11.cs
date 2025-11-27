using Spine.Unity;
using UnityEngine;

public class Level_11 : LevelBase
{
    public SkeletonAnimation skeletonCapy;
    public SkeletonAnimation skeletonB1;
    public SkeletonAnimation skeletonB2;

    private bool capyPlaced;
    private bool bird1Placed;
    private bool bird2Placed;

    public override void Init()
    {
        base.Init();
        skeletonB1.gameObject.SetActive(false);
        skeletonB2.gameObject.SetActive(false);
        skeletonCapy.gameObject.SetActive(false);
        
        capyPlaced = false;
        bird1Placed = false;
        bird2Placed = false;
    }

    protected override ItemSlot CreateItemSlotInstance(GameObject go)
    {
        return go.AddComponent<Slot_11>();
    }
    
    public void HandleAnimationCapy()
    {
        capyPlaced = true;
        skeletonCapy.gameObject.SetActive(true);
        string currentAnim = "c_idle1";
        
        if (bird1Placed || bird2Placed)
        {
            currentAnim = "c_idle2"; 
            skeletonCapy.AnimationState.SetAnimation(1, "food", false); // Ăn
            
            if (bird1Placed) skeletonB1.AnimationState.SetAnimation(0, "b1_idle2", true);
            if (bird2Placed) skeletonB2.AnimationState.SetAnimation(0, "b2_idle2", true);
        }

        skeletonCapy.AnimationState.SetAnimation(0, currentAnim, true);
    }
    
    public void HandleAnimationBird2()
    {
        bird2Placed = true;
        skeletonB2.gameObject.SetActive(true);

        if (capyPlaced)
        {
            skeletonB2.AnimationState.SetAnimation(0, "b2_idle2", true);
            
            skeletonCapy.AnimationState.SetAnimation(0, "c_idle2", true);
            skeletonCapy.AnimationState.SetAnimation(1, "food", false);
        }
        else
        {
            skeletonB2.AnimationState.SetAnimation(0, "b2_idle1", true);
        }
    }
    
    public void HandleAnimationBird1()
    {
        bird1Placed = true;
        skeletonB1.gameObject.SetActive(true);

        if (capyPlaced)
        {
            skeletonB1.AnimationState.SetAnimation(0, "b1_idle2", true);
            
            skeletonCapy.AnimationState.SetAnimation(0, "c_idle2", true);
            skeletonCapy.AnimationState.SetAnimation(1, "food", false);
        }
        else
        {
            skeletonB1.AnimationState.SetAnimation(0, "b1_idle1", true);
        }
    }
}