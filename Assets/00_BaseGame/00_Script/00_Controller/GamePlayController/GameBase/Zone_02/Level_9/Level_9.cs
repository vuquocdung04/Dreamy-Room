using System;
using Cysharp.Threading.Tasks;
using Spine.Unity;
using UnityEngine;

public class Level_9 : LevelBase
{
    public SkeletonAnimation caoSkeleton;

    public override void Init()
    {
        base.Init();
        caoSkeleton.gameObject.SetActive(false);
    }

    protected override ItemSlot CreateItemSlotInstance(GameObject go)
    {
        return go.AddComponent<Slot_9>();
    }

    public async UniTask HandleCaoSlayAnimation()
    {
        caoSkeleton.gameObject.SetActive(true);
        var trackEntry = caoSkeleton.AnimationState.SetAnimation(0, "0-start", false);
        float duration = trackEntry.Animation.Duration;
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        caoSkeleton.AnimationState.SetAnimation(0, "1-loop", true);
    }
}