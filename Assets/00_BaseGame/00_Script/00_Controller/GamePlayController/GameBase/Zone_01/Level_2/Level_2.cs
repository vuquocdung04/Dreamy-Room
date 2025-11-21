
using System;
using Cysharp.Threading.Tasks;
using Spine.Unity;
using UnityEngine;

public class Level_2 : LevelBase
{
    public SkeletonAnimation skeleton;
    public Transform bathIdle;

    public override void Init()
    {
        base.Init();
        skeleton.gameObject.SetActive(false);
    }

    protected override ItemSlot CreateItemSlotInstance(GameObject go)
    {
        return go.AddComponent<Slot_2>();
    }
    protected override async UniTask OnBeforeWinCompleted()
    {
        await base.OnBeforeWinCompleted();
        var trackEntry = skeleton.AnimationState.SetAnimation(0, "2-add-duck-capi", false);
        float duration = trackEntry.Animation.Duration;
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        var trackEntry2 = skeleton.AnimationState.SetAnimation(0, "2-add-duck-loop", true);
        float duration2 = trackEntry2.Animation.Duration;
        await UniTask.Delay(TimeSpan.FromSeconds(duration2));
    }
    public void HandleBathFillWater()
    {
        bathIdle.gameObject.SetActive(false);
        skeleton.gameObject.SetActive(true);
        var trackEntry = skeleton.AnimationState.SetAnimation(0, "1-Bath-fill-water-anim", false);
        trackEntry.Complete += (t) =>
        {
            skeleton.AnimationState.SetAnimation(0, "1-Bath-fill-water-idle", true);
        };
    }
}