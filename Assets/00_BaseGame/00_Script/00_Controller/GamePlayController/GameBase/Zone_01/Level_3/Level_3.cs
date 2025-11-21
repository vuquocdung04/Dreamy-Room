using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class Level_3 : LevelBase
{
    [Header("Animations")]
    public SkeletonAnimation skeletonAnimation;
    protected override ItemSlot CreateItemSlotInstance(GameObject go)
    {
        return go.AddComponent<Slot_3>();
    }

    protected override async UniTask OnBeforeWinCompleted()
    {
        await base.OnBeforeWinCompleted();
        skeletonAnimation.gameObject.SetActive(true);
        var trackEntry = skeletonAnimation.AnimationState.SetAnimation(0,"action1",false);
        float duration = trackEntry.Animation.Duration;
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        var trackEntry2 = skeletonAnimation.AnimationState.SetAnimation(0, "action2", true);
        float duration2 = trackEntry2.Animation.Duration;
        await UniTask.Delay(TimeSpan.FromSeconds(duration2));
    }


}