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
    [SerializeField] protected Sprite sprPlaced;
    [SerializeField] protected Sprite sprUpdate;
    [Tooltip("Item sẽ đổi sprite (sang sprPlaced) khi item này được đặt đúng.")] [SerializeField]
    protected ItemBase targetItemToUpdate;

    [SerializeField] protected bool isInteractableAfterPlacement;
    [SerializeField] protected bool isPlaced;
    private Tween idleTween;
    private Tween restoreIdleTween;
    private Vector3 originalScale;
    
    private Vector3 newPosition;
    private bool toggleChangAnim;
    public int GetIndexLayer() => indexLayer;

    public void AddSnapSlot(ItemSlot slot)
    {
        slotsSnap.Clear();
        slotsSnap.Add(slot);
    }

    public Sprite GetSprite() => spriteRenderer.sprite;

    #endregion
    public void Init(Transform pos)
    {
        originalScale = transform.localScale;
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
        transform.DOScale(originalScale, 0.3f).SetEase(Ease.OutBack);
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
        coll2D.enabled = false;
        StopIdleTween();
        targetSlot.isFullSlot = true;
        spriteRenderer.sortingOrder = slotsSnap.Count >= 2 && targetSlot.HasSpriteRenderer()
            ? targetSlot.SetOrderItemPlaced() + 1
            : indexLayer;
        spriteRenderer.sortingLayerName = SortingLayerName.DEFAULT;
        gameObject.layer = LayerMask.NameToLayer(LayerMaskName.DEFAULT);
        transform.DORotate(Vector3.zero, 0.2f);
        transform.DOMove(targetSlot.transform.localPosition, 0.5f).OnComplete(delegate
        {
            if (targetItemToUpdate == null)
                targetSlot.SetActive();
            if (isInteractableAfterPlacement)
                coll2D.enabled = true;
            UpdateTargetItemSpriteAfterPlacement();
            UpdateSpriteToPlaced();

            // ReSharper disable once SuspiciousTypeConversion.Global
            if (this is IPostPlacementAction specialItem)
            {
                specialItem.HandlePostPlacementAction();
            }
            this.PostEvent(EventID.SPAWN_STAR, this);
        });
        this.PostEvent(EventID.ITEM_PLACED_CORRECTLY, this);
        this.PostEvent(EventID.ON_BOOSTER_CONDITION_CHANGED);
        isPlaced = true;
    }

    
    
    private void UpdateSpriteToPlaced()
    {
        if (sprPlaced == null) return;
        spriteRenderer.sprite = sprPlaced;
    }

    private void UpdateTargetItemSpriteAfterPlacement()
    {
        if (!targetItemToUpdate) return;
        if (targetItemToUpdate.sprUpdate == null) return;
        gameObject.SetActive(false);
        targetItemToUpdate.spriteRenderer.sprite = targetItemToUpdate.sprUpdate;
    }

    private void OnFailSnap()
    {
        spriteRenderer.sortingOrder = indexLayer;
        transform.eulerAngles = new Vector3(0, 0, angle);
        RestoreIdleState();
    }

    #endregion

    #region Drag & Drop

    public void OnStartDrag(float top, Vector3 mousePosition)
    {
        if (!isPlaced)
        {
            spriteRenderer.sortingOrder = 100;
            StopIdleTween();
            StopRestoreIdleTween();
            transform.DORotate(Vector3.zero, 0.2f);

            if (itemSize == ItemSize.Small)
            {
                var newPos = mousePosition;
                newPos.y += 2f;
                if (newPos.y >= top)
                {
                    newPos.y = top;
                }

                transform.position = newPos;
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

    private void RestoreIdleState()
    {
        restoreIdleTween = transform.DORotate(new Vector3(0, 0, angle), 0.2f).OnComplete(PlayIdleTween);
    }

    private void StopRestoreIdleTween()
    {
        if (restoreIdleTween == null) return;
        restoreIdleTween.Kill();
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

    #endregion

    #region Setup

    public void SetupOdin()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll2D = GetComponent<Collider2D>();
        indexLayer = spriteRenderer.sortingOrder;
        spriteRenderer.sortingLayerName = SortingLayerName.ITEM_UNPLACED;
    }

    public void SetStateItem()
    {
        if (conditionSlots.Count > 0) isUnlocked = false;

        if (sprOriginal == null || sprAnim == null)
            isInteractableAfterPlacement = false;
        else
            isInteractableAfterPlacement = true;
    }

    #endregion
}