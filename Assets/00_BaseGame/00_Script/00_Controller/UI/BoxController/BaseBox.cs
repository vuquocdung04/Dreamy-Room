
using DG.Tweening;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public abstract class BaseBox : MonoBehaviour
{
    [SerializeField] protected RectTransform mainPanel;
    [SerializeField] protected bool isAnim = true;
    [SerializeField] protected bool hasAppearedBefore;
    [SerializeField] protected float durationAppeared = 0.2f;
    protected virtual void OnEnable()
    {
        if (!hasAppearedBefore)
        {
            DoFirstAppear();
            hasAppearedBefore = true;
        }
        else
        {
            DoAppear();
        }
    }

    //Ghi de de chay anim ban dau
    protected virtual void DoFirstAppear()
    {
        DoAppear();
    }

    protected virtual void DoAppear()
    {
        if (isAnim)
        {
            mainPanel.localScale = Vector3.zero;
            OnStart();
        }
        else mainPanel.localScale = Vector3.one;
    }

    protected virtual void OnStart()
    {
        mainPanel.DOScale(Vector3.one, durationAppeared);
    }
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}
