
using DG.Tweening;
using UnityEngine;

public abstract class PreGameItem : MonoBehaviour
{
    [SerializeField] protected Collider2D coll2D;
    private float angleZ;
    private Vector3 newPosition;
    protected Transform Target;
    public virtual void Init()
    {
        angleZ = transform.localEulerAngles.z;
        if (coll2D == null)
            coll2D.GetComponent<Collider2D>();
    }

    private void CheckItemPlacement(float threshold)
    {
        var distance = Vector3.Distance(transform.position,Target.position);
        if (distance <= threshold && transform.position.y > Target.position.y)
        {
            OnDoneSnap();
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, angleZ);
        }
    }

    public void OnStartDrag(float top, Vector3 mousePosition)
    {
        transform.DORotate(Vector3.zero, 0.2f).SetEase(Ease.OutSine);
        Vector3 pos = mousePosition;
        pos.y += 1f;
        if (pos.y > top)
            pos.y = top;
        transform.position = pos;
        
    }
    
    public void OnDrag(Vector3 delta, float left, float right, float bottom, float top)
    {
        newPosition = transform.position + delta;
        newPosition.x = Mathf.Clamp(newPosition.x, left, right);
        newPosition.y = Mathf.Clamp(newPosition.y, bottom, top);
        transform.position = newPosition;
    }

    public void OnEndDrag(float threshHold)
    {
        CheckItemPlacement(threshHold);
    }

    protected abstract void OnDoneSnap();
    
    private void OnDestroy()
    {
        transform.DOKill();
    }
}