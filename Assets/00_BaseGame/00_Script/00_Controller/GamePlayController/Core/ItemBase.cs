using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using EventDispatcher;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    #region Variables

    [Header("Behavior Settings")]
    [Tooltip("Vật có thể đặt được ngay từ đầu không? Tắt nếu nó cần được mở khóa bởi vật khác.")]
    [SerializeField]
    protected bool isUnlocked = true;

    [Tooltip("DANH SÁCH các slot mục tiêu mà vật này có thể snap vào")] [SerializeField]
    protected List<ItemSlot> slotsSnap;

    [Space(5)] [SerializeField] protected List<ItemSlot> conditionSlots;

    [Space(5)] [Header("Visuals & Physics")] [SerializeField]
    protected ItemSize itemSize;

    [SerializeField] protected int indexLayer;
    [SerializeField] protected float angle;
    [SerializeField] protected Collider2D coll2D;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Sprite sprOriginal;
    [SerializeField] protected Sprite sprAnim;

    [SerializeField] protected bool isInteractableAfterPlacement;
    [SerializeField] protected bool isPlaced;
    private Tween idleTween;
    private Vector3 newPosition;
    private bool toggleChangAnim;
    public int GetIndexLayer() => indexLayer;

    #endregion

    public void Init(Transform pos)
    {
        transform.localPosition = pos.position;
        gameObject.SetActive(false);
    }


    #region Properties

    public List<ItemSlot> GetTargetSlot() => slotsSnap;

    #endregion

    #region Item Placement

    public void OutSideBox(Vector2 posSpawn)
    {
        coll2D.enabled = false;
        transform.position = posSpawn;
        transform.localScale = Vector3.zero;
        float angleZ = Random.Range(-40f, 40f);
        angle = angleZ;
        transform.localEulerAngles = new Vector3(0, 0, angle);
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        float randY = Random.Range(4f, 6f);
        float randX = Random.Range(-3.5f, 3.5f);
        transform.DOMove(new Vector3(randX, randY), 0.2f).OnComplete(delegate
        {
            coll2D.enabled = true;
            PlayIdleTween();
        });
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
                continue;

            float distance = Vector2.Distance(transform.position, slot.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                bestSlot = slot;
            }
        }

        // Kiểm tra xem slot tốt nhất tìm được có đủ gần không
        if (bestSlot != null && minDistance <= threshold)
            OnDoneSnap(bestSlot);
        else
            OnFailSnap();
    }

    public void OnDoneSnap(ItemSlot targetSlot)
    {
        if (isInteractableAfterPlacement)
        {
            //NOTE: lam gi do
        }
        else
        {
            coll2D.enabled = false;
            StopIdleTween();
            targetSlot.isFullSlot = true;
            spriteRenderer.sortingOrder = slotsSnap.Count >= 2 && targetSlot.HasSpriteRenderer()
                ? targetSlot.SetOrderItemPlaced() + 1
                : indexLayer;
            spriteRenderer.sortingLayerName = SortingLayerName.DEFAULT;
            transform.DORotate(Vector3.zero, 0.2f);
            transform.DOMove(targetSlot.transform.position, 0.5f).OnComplete(targetSlot.Active);
            this.PostEvent(EventID.ITEM_PLACED_CORRECTLY, this);
            this.PostEvent(EventID.ON_BOOSTER_CONDITION_CHANGED);
        }

        isPlaced = true;
    }

    private void OnFailSnap()
    {
        spriteRenderer.sortingOrder = indexLayer;
        transform.eulerAngles = new Vector3(0, 0, angle);
        transform.DORotate(new Vector3(0, 0, angle), 0.2f).OnComplete(PlayIdleTween);
    }

    #endregion

    #region Drag & Drop

    public void OnStartDrag(float top, Vector3 mousePosition)
    {
        if (!isPlaced)
        {
            spriteRenderer.sortingOrder = 100;
            StopIdleTween();
            transform.DORotate(Vector3.zero, 0.2f);

            if (itemSize == ItemSize.Small)
            {
                Vector3 pos = mousePosition;
                pos.y += 2f;
                if (pos.y > top)
                    pos.y = top;
                transform.position = pos;
            }
        }
        else
        {
            if (!isInteractableAfterPlacement) return;
            toggleChangAnim = !toggleChangAnim;
            spriteRenderer.sprite = toggleChangAnim ? sprAnim : sprOriginal;
        }
    }

    public void OnDrag(Vector3 delta, float left, float right, float bottom, float top)
    {
        if (isPlaced) return;

        newPosition = transform.position + delta;
        newPosition.x = Mathf.Clamp(newPosition.x, left, right);
        newPosition.y = Mathf.Clamp(newPosition.y, bottom, top);
        transform.position = newPosition;
    }

    public void OnEndDrag(float threshold)
    {
        if (isPlaced) return;

        CheckItemPlacement(threshold);
    }

    #endregion

    #region Animation

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

    #endregion

    #region Setup

    public void SetupOdin()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll2D = GetComponent<Collider2D>();
        indexLayer = spriteRenderer.sortingOrder;
        spriteRenderer.sortingLayerName = SortingLayerName.ITEM_UNPLACED;
        if (conditionSlots.Count > 0) isUnlocked = false;

        if (sprOriginal == null || sprAnim == null)
            isInteractableAfterPlacement = false;
        else
            isInteractableAfterPlacement = true;
    }

    #endregion
}