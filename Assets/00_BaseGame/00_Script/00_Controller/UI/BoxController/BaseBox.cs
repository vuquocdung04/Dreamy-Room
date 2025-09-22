
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public abstract class BaseBox : MonoBehaviour
{
    [SerializeField] protected RectTransform mainPanel;
    [SerializeField] protected bool isAnim = true;
    [SerializeField]
    [ShowIf("isAnim")]
    protected float durationAppeared = 0.3f;
    [SerializeField] protected bool hasAppearedBefore;
    
    [ShowIf("hasAppearedBefore")]
    [BoxGroup("Sliding Durations")]
    [SerializeField] 
    protected float durationSlide = 0.2f;
    protected virtual void OnEnable()
    {
        if (hasAppearedBefore)
        {
            DoFirstAppear();
            hasAppearedBefore = true;
        }
        else
        {
            HandleAnim();
        }
    }

    //Ghi de de chay anim ban dau
    protected virtual void DoFirstAppear()
    {
        mainPanel.localScale = Vector3.one;
    }

    private void HandleAnim()
    {
        if (isAnim)
        {
            mainPanel.localScale = Vector3.zero;
            DoAppearAnimation();
        }
        else mainPanel.localScale = Vector3.one;
    }

    protected virtual void DoAppearAnimation()
    {
        mainPanel.DOScale(Vector3.one, durationAppeared).SetEase(Ease.OutBack);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
    
    public Tween ShowSliding(bool slideInFromLeft)
    {
        Show();
        Vector2 startPos = new Vector2(slideInFromLeft ? -mainPanel.rect.width : mainPanel.rect.width, 0);
        mainPanel.anchoredPosition = startPos;
        return mainPanel.DOAnchorPos(Vector2.zero, durationSlide).SetEase(Ease.OutCubic);
    }
    public Tween CloseSliding(bool slideOutToLeft)
    {
        Vector2 endPos = new Vector2(slideOutToLeft ? -mainPanel.rect.width : mainPanel.rect.width, 0);
        return mainPanel.DOAnchorPos(endPos, durationSlide/2).SetEase(Ease.InCubic)
            .OnComplete(() =>
            {
                Close();
            });
    }
}
