
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [Header("Fx")]
    public FxStarPrefab fxStarPrefab;

    public float spawnCount = 10f;
    public float spawnRadius = 1f;
    public float moveDistance = 1f;
    public float fxMoveDuration = 0.5f;
    [Header("Star")]
    public float scaleDuration = 0.4f;
    public float moveDuration = 0.5f;
    public Transform starPrefab;
    [Header("Congratulations")]
    public CongratulationPrefab congratulationPrefab;
    public List<Sprite> lsCongratulations;

    public void FxEffect(Vector3 centerPoint)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            var randomOffset = Random.insideUnitCircle * spawnRadius;
            var spawnPosition = centerPoint + new Vector3(randomOffset.x, randomOffset.y);
            
            var effectClone = SimplePool2.Spawn(fxStarPrefab,spawnPosition,Quaternion.identity);
            
            float randScale = Random.Range(0.4f, 0.8f);
            effectClone.Init(randScale);
            
            var direction = (spawnPosition - centerPoint).normalized;
            var targetPosition = spawnPosition + direction * moveDistance;
            
            var seq =  DOTween.Sequence();
            seq.Append(effectClone.transform.DOMove(targetPosition, fxMoveDuration).SetEase(Ease.OutQuad));
            seq.Join(effectClone.spr.DOFade(0f, fxMoveDuration).SetEase(Ease.InQuad));

            seq.OnComplete(delegate
            {
                SimplePool2.Despawn(effectClone.gameObject);
            });
        }
    }
    
    
    public void CongratulationEffect(Vector3 spawnPos)
    {
        var congratulationClone = SimplePool2.Spawn(congratulationPrefab, spawnPos, Quaternion.identity);
        congratulationClone.Random(lsCongratulations[Random.Range(0,lsCongratulations.Count)]);
        var mySequence = DOTween.Sequence();
        var trans = congratulationClone.transform;
        var spr = congratulationClone.spr;
        
        mySequence.Append(trans.DOMoveY(spawnPos.y + 1f,2f).SetEase(Ease.OutBack));
        mySequence.Join(spr.DOFade(0f,2.25f).SetEase(Ease.OutBack));
        mySequence.OnComplete(delegate
        {
            SimplePool2.Despawn(congratulationClone.gameObject);
        });

    }
    
    public void StarEffect(Vector3 spawnPos, Vector3 worldTargetPos, System.Action callback = null)
    {
        var starClone = SimplePool2.Spawn(starPrefab, spawnPos, Quaternion.identity);
        starClone.localScale = Vector3.zero;

        // --- 1. Tạo Sequence ---
        var starSequence = DOTween.Sequence();
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
            callback?.Invoke();
        });
    }
}