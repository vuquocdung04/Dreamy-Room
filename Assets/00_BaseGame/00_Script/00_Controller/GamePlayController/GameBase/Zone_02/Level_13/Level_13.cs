using Cysharp.Threading.Tasks;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class Level_13 : LevelBase
{
    public Transform holderMoon;
    public SpriteRenderer rightCloud;
    public SpriteRenderer leftCloud;

    public SkeletonAnimation birdBlue;
    public SkeletonAnimation birdPink;

    public Transform chairBlue;
    public Transform chairPink;

    private bool birdBluePlaced;
    private bool birdPinkPlaced;

    public override void Init()
    {
        base.Init();
        holderMoon.gameObject.SetActive(false);
        birdBlue.gameObject.SetActive(false);
        birdPink.gameObject.SetActive(false);
    }

    protected override ItemSlot CreateItemSlotInstance(GameObject go)
    {
        return go.AddComponent<Slot_13>();
    }

    protected override async UniTask OnBeforeWinCompleted()
    {
        await base.OnBeforeWinCompleted();

        holderMoon.gameObject.SetActive(true);
        var posR = rightCloud.transform.localPosition;
        var posL = leftCloud.transform.localPosition;
        var seq = DOTween.Sequence();
        var s1 = seq.Append(rightCloud.transform.DOLocalMoveX(posR.x + 2f, 1f));
        var s2 = seq.Join(rightCloud.DOFade(0f, 1f));
        var s3 = seq.Join(leftCloud.transform.DOLocalMoveX(posL.x - 2f, 1f));
        var s4 = seq.Join(leftCloud.DOFade(0f, 1f));
        await seq.ToUniTask();
    }

    public void BirdBlueAnim()
    {
        birdBluePlaced = true;
        chairBlue.gameObject.SetActive(false);
        birdBlue.gameObject.SetActive(true);
        
        birdBlue.AnimationState.SetAnimation(0, "1-bird-start", false);
        
        if (birdPinkPlaced)
        {
            birdBlue.AnimationState.AddAnimation(0, "1-bird-loop", true, 0);
            
            birdPink.AnimationState.SetAnimation(0, "1-chim-loop", true);
        }
    }

    public void BirdPinkAnim()
    {
        birdPinkPlaced = true;
        chairPink.gameObject.SetActive(false);
        birdPink.gameObject.SetActive(true);

        birdPink.AnimationState.SetAnimation(0, "1-chim-start", false);

        if (birdBluePlaced)
        {
            birdPink.AnimationState.AddAnimation(0, "1-chim-loop", true, 0);

            birdBlue.AnimationState.SetAnimation(0, "1-bird-loop", true);
        }
    }
}