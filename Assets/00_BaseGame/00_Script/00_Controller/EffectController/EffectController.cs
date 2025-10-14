using DG.Tweening;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public Transform starPrefab; //Prefab
    public float scaleDuration = 0.4f;
    public float moveDuration = 0.5f;
    public void StarEffect(Vector3 spawnPos, Vector3 worldTargetPos)
    {
        var starClone = SimplePool2.Spawn(starPrefab, spawnPos, Quaternion.identity);
        starClone.localScale = Vector3.zero;

        // --- 1. Tạo Sequence ---
        Sequence starSequence = DOTween.Sequence();
        starSequence.Append(
            starClone.DOScale(Vector3.one * 0.75f, scaleDuration)
                     .SetEase(Ease.OutBounce) 
        );
        starSequence.Append(
            starClone.DOMove(worldTargetPos, moveDuration)
                     .SetEase(Ease.Linear)
        );
        starSequence.Join(
            starClone.DORotate(new Vector3(0, 0, -720), moveDuration, RotateMode.FastBeyond360) 
                     .SetEase(Ease.Linear)
        );
        starSequence.OnComplete(() =>
        {
            SimplePool2.Despawn(starClone.gameObject);
        });
    }
}