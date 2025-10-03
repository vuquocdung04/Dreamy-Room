
using DG.Tweening;
using UnityEngine;

public abstract class PreGameItem : MonoBehaviour
{
    [SerializeField] private Collider2D coll2D;
    private float angleZ;
    private Vector3 newPosition;
    public void Init()
    {
        angleZ = transform.localEulerAngles.z;
    }

    private void CheckItemPlacement(Collider2D placement, System.Action callback)
    {
        if (coll2D.IsTouching(placement))
        {
            OnDoneSnap();
            callback?.Invoke();
        }
        else
        {
            transform.DOMove(new Vector3(0,0,angleZ), 0.2f).SetEase(Ease.OutSine);
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

    public void OnEndDrag(Collider2D placement, System.Action callback = null)
    {
        CheckItemPlacement(placement, callback);
    }

    protected abstract void OnDoneSnap();
    
    private void OnDestroy()
    {
        transform.DOKill();
    }
}