
using DG.Tweening;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public abstract class BaseBox : MonoBehaviour
{
    [SerializeField] protected RectTransform mainPanel;
    [SerializeField] protected bool isAnim = true;
    [SerializeField] protected bool hasAppearedBefore;
    [SerializeField] protected float durationAppeared = 0.2f;
    [SerializeField] protected float durationDisappeared = 0.2f;
    protected virtual void OnEnable()
    {
        if (!hasAppearedBefore)
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
        mainPanel.DOScale(Vector3.one, durationAppeared);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
    
    public void ShowSliding(bool slideInFromLeft)
    {
        Show();
        Vector2 startPos = new Vector2(slideInFromLeft ? -mainPanel.rect.width : mainPanel.rect.width, 0);
        mainPanel.anchoredPosition = startPos;
        mainPanel.DOAnchorPos(Vector2.zero, durationAppeared).SetEase(Ease.OutCubic);
    }
    public void CloseSliding(bool slideOutToLeft)
    {
        Vector2 endPos = new Vector2(slideOutToLeft ? -mainPanel.rect.width : mainPanel.rect.width, 0);
        mainPanel.DOAnchorPos(endPos, durationDisappeared).SetEase(Ease.InCubic)
            .OnComplete(() =>
            {
                Close();
            });
    }
}
