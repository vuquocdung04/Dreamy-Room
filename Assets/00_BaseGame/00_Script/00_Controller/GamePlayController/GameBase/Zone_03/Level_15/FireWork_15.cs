using System;
using DG.Tweening;
using UnityEngine;

public class FireWork_15 : MonoBehaviour
{
    public SpriteRenderer spr;

    private Tween tween;
    
    public void HandleAction(Sprite sp, float randScale)
    {
        spr.sprite = sp;
        spr.color = Color.white;
        transform.localScale = Vector3.one * randScale;
        tween = spr.DOFade(0f, 1f).SetEase(Ease.OutBack).OnComplete(delegate
        {
            SimplePool2.Despawn(gameObject);
        });
    }

    private void OnDestroy()
    {
        tween.Kill();
        tween = null;
    }
}