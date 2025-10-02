
using DG.Tweening;
using UnityEngine;

public class LevelTut : LevelBase
{
    public Transform charSleep;
    private Tween sleepTween;

    public override void Init()
    {
        base.Init();
        StartTween();
    }

    private void StartTween()
    {
        sleepTween = charSleep.DOScale(Vector3.one * 0.7f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    private void OnDestroy()
    {
        if (sleepTween != null)
        {
            sleepTween.Kill();
            sleepTween = null;
        }
    }
}