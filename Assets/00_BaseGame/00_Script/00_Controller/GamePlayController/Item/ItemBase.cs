using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    [Header("Behavior Settings")]
    [Tooltip("Vật có thể đặt được ngay từ đầu không? Tắt nếu nó cần được mở khóa bởi vật khác.")]
    [SerializeField]
    protected bool isUnlocked = true;

    [Tooltip("DANH SÁCH các slot mục tiêu mà vật này có thể snap vào")]
    [SerializeField]
    protected List<ItemSlot> slotsSnap;

    [Space(5)]
    [Header("Visuals & Physics")]
    [SerializeField]
    protected ItemSize itemSize;
    [SerializeField] protected int indexLayer;
    [SerializeField] protected float angle;
    [SerializeField] protected Collider2D coll2D;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Sprite sprOriginal;
    [SerializeField] protected Sprite sprAnim;

    private Tween idleTween;

    public List<ItemSlot> GetTargetSlot() => slotsSnap;
    private void CheckItemPlacement(float threshold)
    {
        if (!isUnlocked)
        {
            OnFailSnap();
            return;
        }

        if (slotsSnap == null || slotsSnap.Count == 0)
        {
            OnFailSnap();
            return;
        }

        ItemSlot bestSlot = null;
        float minDistance = float.MaxValue;

        // Luôn lặp qua danh sách slot để tìm ra vị trí phù hợp nhất
        foreach (var slot in slotsSnap)
        {
            if (slot == null || slot.isFullSlot)
            {
                continue;
            }

            float distance = Vector2.Distance(transform.position, slot.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                bestSlot = slot;
            }
        }

        // Kiểm tra xem slot tốt nhất tìm được có đủ gần không
        if (bestSlot != null && minDistance <= threshold)
        {
            OnDoneSnap(bestSlot);
        }
        else
        {
            OnFailSnap();
        }
    }

    public void OnDoneSnap(ItemSlot targetSlot)
    {
        StopIdleTween();
        coll2D.enabled = false;
        targetSlot.isFullSlot = true;
        targetSlot.Active();
        transform.DOMove(targetSlot.transform.position, 0.5f);
    }

    private void OnFailSnap()
    {
        spriteRenderer.sortingOrder = indexLayer;
        transform.eulerAngles = new Vector3(0, 0, angle);
        PlayIdleTween();
    }

    public virtual void OnDrag(Vector3 delta)
    {
        transform.position += delta;
        switch (itemSize)
        {
            case ItemSize.Small:
                transform.position += new Vector3(delta.x, delta.y, 0);
                break;
            case ItemSize.Large:
                transform.position += delta;
                break;
        }
    }

    public virtual void OnEndDrag(float threshold)
    {
        CheckItemPlacement(threshold);
    }

    public virtual void OnStartDrag()
    {
        spriteRenderer.sortingOrder = 100;
        StopIdleTween();
    }

    private void PlayIdleTween()
    {
        if (this == null || !gameObject.activeInHierarchy) return;
        var posY = transform.position.y;
        idleTween = transform.DOMoveY(posY + 0.3f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    private void StopIdleTween()
    {
        if (idleTween != null)
            idleTween.Kill();
    }
}