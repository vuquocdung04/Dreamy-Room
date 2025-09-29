using DG.Tweening;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    [Header("Setting Tutorial")] [SerializeField]
    protected bool isTut;

    [SerializeField] protected Vector2 tutSnapPosition;
    [Space(5)] [SerializeField] protected ItemSize itemSize;
    [SerializeField] protected bool isUnlocked = true;
    [SerializeField] protected ItemSlot slotSnap;
    [SerializeField] protected int indexLayer;
    [SerializeField] protected float angle;
    [SerializeField] protected Collider2D coll2D;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Sprite sprOriginal;
    [SerializeField] protected Sprite sprAnim;
    private Tween idleTween;

    private void CheckItemPlacement(float threshold)
    {
        if (!isUnlocked) return;

        Vector2 targetPosition;
        if (isTut)
            targetPosition = tutSnapPosition;
        else
        {
            if (slotSnap == null)
            {
                OnFailSnap();
                return;
            }

            targetPosition = slotSnap.transform.position;
        }

        var distance = Vector2.Distance(this.transform.position, targetPosition);
        if (distance <= threshold)
            OnDoneSnap();
        else
            OnFailSnap();
    }

    private void OnFailSnap()
    {
        spriteRenderer.sortingOrder = indexLayer;
        transform.eulerAngles = new Vector3(0, 0, angle);
        PlayIdleTween();
    }

    private void OnDoneSnap()
    {
        StopIdleTween();
        coll2D.enabled = false;
        Vector2 targetPosition = isTut ? tutSnapPosition : slotSnap.transform.position;
        transform.DOMove(targetPosition, 0.5f);
    }

    private void PlayIdleTween()
    {
        var posY = transform.position.y;
        idleTween = transform.DOMoveY(posY + 3f, 0.2f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    private void StopIdleTween()
    {
        if (idleTween != null)
            idleTween.Kill();
    }

    public virtual void OnEndDrag(int threshold)
    {
        CheckItemPlacement(threshold);
    }

    public virtual void OnStartDrag()
    {
        spriteRenderer.sortingOrder = 100;
        StopIdleTween();
    }

    public virtual void OnDrag(Vector3 delta)
    {
        transform.position += delta;
        switch (itemSize)
        {
            case ItemSize.Small:
                transform.position += new Vector3(delta.x, delta.y + 2, 0);
                break;
            case  ItemSize.Large:
                transform.position += delta;
                break;
        }
    }
}