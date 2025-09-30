using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using EventDispatcher;
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
    [SerializeField] protected List<ItemSlot> conditionSlots;
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


    public void OutSideBox(Vector2 posSpawn)
    {
        transform.position = posSpawn;
        transform.localScale = Vector3.zero;
        float angleZ = Random.Range(-40f, 40f);
        angle = angleZ;
        transform.localEulerAngles = new Vector3(0, 0, angle);
        transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        float randY = Random.Range(1.5f, 6f);
        float randX = Random.Range(-3.5f, 3.5f);
        transform.DOMove(new Vector3(randX, randY), 0.2f).OnComplete(PlayIdleTween);
    }
    public void ValidateUnlockState()
    {
        if (isUnlocked) return;
        
        if (conditionSlots == null || conditionSlots.Count == 0) return;
        
        bool allConditionsMet = conditionSlots.All(slot => slot != null && slot.isFullSlot);
        
        if (allConditionsMet)
            isUnlocked = true;
    }
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
        targetSlot.Active();
        targetSlot.isFullSlot = true;
        transform.DOMove(targetSlot.transform.position, 0.5f);
        this.PostEvent(EventID.UPDATE_UNLOCK_ITEM);
    }

    private void OnFailSnap()
    {
        spriteRenderer.sortingOrder = indexLayer;
        transform.eulerAngles = new Vector3(0, 0, angle);
        transform.DORotate(new Vector3(0,0,angle), 0.2f).OnComplete(PlayIdleTween);
    }

    public void OnDrag(Vector3 delta)
    {
        transform.position += delta;
        transform.DORotate(Vector3.zero, 0.2f);
    }

    public void OnEndDrag(float threshold)
    {
        CheckItemPlacement(threshold);
    }

    public void OnStartDrag()
    {
        spriteRenderer.sortingOrder = 100;
        StopIdleTween();
        if (itemSize == ItemSize.Small)
        {
            Vector3 pos = transform.position;
            pos.y += 1f;
            transform.position = pos;
        }
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

    /// <summary>
    ///  Setup
    /// </summary>
    public void SetupOdin()
    {
        spriteRenderer =  GetComponent<SpriteRenderer>();
        coll2D = GetComponent<Collider2D>();
        indexLayer = spriteRenderer.sortingOrder;
        if (conditionSlots.Count > 0) isUnlocked = false;
    }
}