using System;
using Cysharp.Threading.Tasks;
using Spine.Unity;
using UnityEngine;

public class Level_6 : LevelBase
{
    public Transform cupCoffe;
    public SkeletonAnimation bearSkeleton;
    public SkeletonAnimation batmanSkeleton;

    public override void Init()
    {
        base.Init();
        batmanSkeleton.gameObject.SetActive(false);
        bearSkeleton.gameObject.SetActive(false);
    }

    protected override ItemSlot CreateItemSlotInstance(GameObject go)
    {
        return go.AddComponent<Slot_6>();
    }

    protected override async UniTask OnBeforeWinCompleted()
    {
        await base.OnBeforeWinCompleted();
        batmanSkeleton.gameObject.SetActive(true);
        var trackEnty = batmanSkeleton.AnimationState.SetAnimation(0, "1", false);
        var duration = trackEnty.Animation.Duration;
        await UniTask.Delay(TimeSpan.FromSeconds(3f));
        bearSkeleton.AnimationState.SetAnimation(0, "fear", true);
        cupCoffe.gameObject.SetActive(false);
        await UniTask.Delay(TimeSpan.FromSeconds(duration - 3f));
    }

    public void HandleBearAnimation()
    {
        bearSkeleton.gameObject.SetActive(true);
        bearSkeleton.AnimationState.SetAnimation(0, "fun", true);
    }
}