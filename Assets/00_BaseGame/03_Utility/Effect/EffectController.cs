using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EffectController : MonoBehaviour
{
    [Header("Booster Image")]
    [SerializeField] private Image imgBooster;
    [SerializeField] private Image effect;
    [Header("Booster Settings")]
    [SerializeField] private float targetY = 100f;
    [SerializeField] private float boosterDuration = 0.2f;
    [SerializeField] private float effectDuration = 0.25f;
    private Vector3 originalPosition;
    private DataBoosterBase dataBooster;
    private BoosterConflict boosterConflict;
    private GiftType boosterType;
    
    public void Init()
    {
        dataBooster = GameController.Instance.dataContains.dataBooster;
        originalPosition = imgBooster.transform.localPosition;
        imgBooster.transform.gameObject.SetActive(false);
        effect.transform.gameObject.SetActive(false);
    }
    
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
        effect.gameObject.SetActive(true);
        Sequence seq = DOTween.Sequence();
        
        seq.Append(imgBooster.transform.DOLocalMove(new Vector3(originalPosition.x,originalPosition.y + targetY,originalPosition.z), boosterDuration ).SetEase(Ease.OutBack));
        seq.Join(effect.transform.DOScale(Vector3.one * 5f, effectDuration));
        seq.Join(effect.DOFade(0f, effectDuration));

        seq.OnComplete(delegate
        {
            ResetBooster();
            callback?.Invoke();
        });
    }

    private void ResetBooster()
    {
        imgBooster.transform.localPosition = originalPosition;
        effect.transform.localScale = Vector3.one;
        effect.color = Color.white;
        imgBooster.gameObject.SetActive(false);
        effect.gameObject.SetActive(false);
    }
}