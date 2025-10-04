using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class EffectController : MonoBehaviour
{
    [Header("Booster Image")]
    [SerializeField] private Image imgBooster;
    [SerializeField] private Image effect;
    [Header("Booster Settings")]
    [SerializeField] private float targetY = 20f;
    [SerializeField] private float generalDuration = 0.5f;
    private Vector3 originalPosition;
    private DataBoosterBase dataBooster;
    private BoosterConflict boosterConflict;
    private GiftType boosterType;
    
    public void Init()
    {
        dataBooster = GameController.Instance.dataContains.dataBooster;
        originalPosition = imgBooster.transform.localPosition;
    }

    [Button("Effect", ButtonSizes.Large)]
    public void EffectBooster(System.Action callback = null)
    {
        GetBoosterIcon();
        RunEffect(callback);
    }
    
    private void GetBoosterIcon()
    {
        boosterType = dataBooster.boosterTypeSeleced;
        boosterConflict = dataBooster.GetBoosterConflict(boosterType);
        imgBooster.sprite = boosterConflict.GetIcon();
        imgBooster.SetNativeSize();
    }

    private void RunEffect(System.Action callback)
    {
        imgBooster.gameObject.SetActive(true);
        Sequence seq = DOTween.Sequence();
        
        seq.Append(imgBooster.transform.DOLocalMove(new Vector3(originalPosition.x,originalPosition.y + targetY,originalPosition.z), generalDuration ).SetEase(Ease.OutBack));
        seq.Join(effect.transform.DOScale(Vector3.one * 5f, generalDuration));
        seq.Join(effect.DOFade(0f, generalDuration));

        seq.OnComplete(delegate
        {
            ResetBooster();
            callback?.Invoke();
        });
    }

    private void ResetBooster()
    {
        imgBooster.gameObject.SetActive(false);
        imgBooster.transform.localPosition = originalPosition;
        effect.transform.localScale = Vector3.zero;
        effect.color = Color.white;
    }
}