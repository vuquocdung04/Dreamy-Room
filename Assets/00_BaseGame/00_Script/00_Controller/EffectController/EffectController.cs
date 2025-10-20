
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

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
    [Header("Star Spark Effect")]
    public SparkFxPrefab sparkFxPrefab;

    public float sparkSpawnInterval = 0.05f;
    public float sparkMoveDistance = 0.5f;
    public float sparkFadeDuration = 0.2f;
    public float sparkMinScale = 0.2f;
    public float sparkMaxScale = 0.5f;
    
    [Header("Congratulations")]
    public CongratulationPrefab congratulationPrefab;
    public List<Sprite> lsCongratulations;

    public void FxEffect(Vector3 centerPoint)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            var randomOffset = Random.insideUnitCircle * spawnRadius;
            var spawnPosition = centerPoint + new Vector3(randomOffset.x, randomOffset.y);
        
            var effectClone = SimplePool2.Spawn(fxStarPrefab, spawnPosition, Quaternion.identity);
        
            float randScale = Random.Range(0.4f, 0.8f);
            effectClone.Init(randScale);
        
            var direction = (spawnPosition - centerPoint).normalized;
            var targetPosition = spawnPosition + direction * moveDistance;
    
            PlayEffectAsync(effectClone, targetPosition).Forget();
        }
    }

    private async UniTaskVoid PlayEffectAsync(FxStarPrefab effect, Vector3 targetPosition)
    {
        try
        {
            var ct = effect.GetCancellationTokenOnDestroy();
            
            await UniTask.WhenAll(
                effect.transform.DOMove(targetPosition, fxMoveDuration)
                    .SetEase(Ease.OutQuad)
                    .SetRecyclable(true)
                    .SetLink(effect.gameObject)
                    .ToUniTask(cancellationToken: ct),
                
                effect.spr.DOFade(0f, fxMoveDuration)
                    .SetEase(Ease.InQuad)
                    .SetRecyclable(true)
                    .SetLink(effect.gameObject)
                    .ToUniTask(cancellationToken: ct)
            );
        
            SimplePool2.Despawn(effect.gameObject);
        }
        catch (OperationCanceledException)
        {
            
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
    
    public void StarEffect(Vector3 spawnPos, Vector3 worldTargetPos, Action callback = null)
    {
        if (starPrefab == null || sparkFxPrefab == null)
        {
            Debug.LogError("StarPrefab or SparkFxPrefab is not assigned!", this);
            callback?.Invoke();
            return;
        }
        
        var starClone = SimplePool2.Spawn(starPrefab, spawnPos, Quaternion.identity);
        starClone.localScale = Vector3.zero;

        var starSequence = DOTween.Sequence();
        
        starSequence.Append(
            starClone.DOScale(Vector3.one * 0.75f, scaleDuration)
                     .SetEase(Ease.OutBounce)
        );
        
        var moveTween = starClone.DOMove(worldTargetPos, moveDuration).SetEase(Ease.Linear);
        moveTween.OnStart(() =>
        {
            // Bắt đầu quá trình spawn spark khi ngôi sao bắt đầu bay
            Vector3 travelDirection = (worldTargetPos - starClone.position).normalized;
            SpawnSparkAlongPathAsync(starClone, travelDirection, moveDuration).Forget();
        });
        
        starSequence.Append(moveTween);
        
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
    private async UniTaskVoid SpawnSparkAlongPathAsync(Transform starTrans, Vector3 travelDirection, float totalSpawnTime)
    {
        var ct = starTrans.gameObject.GetCancellationTokenOnDestroy();
        float timer = 0f;

        while (timer < totalSpawnTime)
        {
            try
            {
                Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
                Vector3 sparkSpawnPos = starTrans.position + (Vector3)randomOffset;

                Vector3 sparkTargetPos = sparkSpawnPos - travelDirection * sparkMoveDistance;
                
                var sparkClone = SimplePool2.Spawn(sparkFxPrefab, sparkSpawnPos, Quaternion.identity);
                PlaySparkEffectAsync(sparkClone, sparkTargetPos).Forget();

                timer += sparkSpawnInterval;
                await UniTask.Delay(TimeSpan.FromSeconds(sparkSpawnInterval), cancellationToken: ct);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }
    
    private async UniTaskVoid PlaySparkEffectAsync(SparkFxPrefab spark, Vector3 targetPosition)
    {
        try
        {
            var ct = spark.GetCancellationTokenOnDestroy();
            
            spark.transform.localScale = Vector3.one * Random.Range(sparkMinScale, sparkMaxScale);
            spark.spr.color = new Color(spark.spr.color.r, spark.spr.color.g, spark.spr.color.b, 1f);
            
            await UniTask.WhenAll(
                spark.transform.DOMove(targetPosition, sparkFadeDuration)
                    .SetEase(Ease.OutQuad)
                    .SetLink(spark.gameObject)
                    .ToUniTask(cancellationToken: ct),

                spark.spr.DOFade(0f, sparkFadeDuration)
                    .SetEase(Ease.InQuad)
                    .SetLink(spark.gameObject)
                    .ToUniTask(cancellationToken: ct)
            );
            
            SimplePool2.Despawn(spark.gameObject);
        }
        catch (OperationCanceledException)
        {
           
        }
    }
}