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
    protected bool isAvailableForHint = true;

    [SerializeField] protected bool isPlacedByPlayer = true;

    [Tooltip("DANH SÁCH các slot mục tiêu mà vật này có thể snap vào")] [SerializeField]
    protected List<ItemSlot> slotsSnap;

    [SerializeField] protected Transform shadowItem;

    [Space(5)] [Header("Visuals & Physics")] [SerializeField]
    protected ItemSize itemSize;

    [SerializeField] protected int indexLayer;
    [SerializeField] protected float angle;
    [SerializeField] protected Collider2D coll2D;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Sprite sprOriginal;
    [SerializeField] protected Sprite sprAnim;
    [SerializeField] protected Sprite sprPlaced;
    [SerializeField] protected bool isInteractableAfterPlacement;
    [SerializeField] protected bool isPlaced;
    private Tween idleTween;
    private Tween restoreIdleTween;
    private Vector3 originalScale;

    private Vector3 newPosition;
    private bool toggleChangAnim;
    private PlayerContains playerContains;
    public int GetIndexLayer() => indexLayer;
    public bool GetPlacedByPlayer() => isPlacedByPlayer;
    public void SetPlacedByPlayer(bool state) => isPlacedByPlayer = state;
    public bool GetItemPlaced() => isPlaced;
    public int GetCountSnapsSlot() => slotsSnap.Count;

    public void AddSnapSlot(ItemSlot slot)
    {
        slotsSnap.Clear();
        slotsSnap.Add(slot);
    }

    public Sprite GetSprite() => spriteRenderer.sprite;

    #endregion

    public virtual void Init(Transform pos)
    {
        originalScale = transform.localScale;
        transform.localPosition = pos.position;
        gameObject.SetActive(false);
        if (shadowItem)
            shadowItem.gameObject.SetActive(false);
        playerContains = GamePlayController.Instance.playerContains;
    }
    public List<ItemSlot> GetTargetSlot() => slotsSnap;

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
        float randX = playerContains.GetRandomSpawnX();
        transform.DOLocalMove(new Vector3(randX, randY), 0.2f).OnComplete(delegate
        {
            coll2D.enabled = true;
            PlayIdleTween();
        });
    }

    public virtual void ValidateUnlockState()
    {
        if (isAvailableForHint) return;
        if (slotsSnap == null) return;

        isAvailableForHint = slotsSnap.All(slot => slot != null && slot.IsReadyToReceiveItem());
    }

    private void CheckItemPlacement(float threshold)
    {
        if (slotsSnap == null || slotsSnap.Count == 0)
        {
            OnFailSnap();
            return;
        }
        ItemSlot bestSlot = null;
        float minDistance = float.MaxValue;

        foreach (var slot in slotsSnap)
        {
            if (slot == null || slot.isFullSlot)
                continue;

            float distance = Vector2.Distance(transform.position, slot.transform.position);
            if (distance <= minDistance)
            {
                minDistance = distance;
                bestSlot = slot;
            }
        }

        // Kiểm tra xem slot tốt nhất tìm được có đủ gần không
        if (bestSlot != null && minDistance <= threshold)
        {
            if (bestSlot.IsReadyToReceiveItem())
                OnDoneSnap(bestSlot);
            else
                OnFailSnap();
        }
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
        transform.DOLocalMove(targetSlot.transform.localPosition, 0.4f).OnComplete(delegate
        {
            isPlaced = true;
            GameController.Instance.effectController.FxEffect(transform.localPosition);
            if (shadowItem)
                shadowItem.gameObject.SetActive(true);
            targetSlot.SetActive();
            if(targetSlot.gameObject.activeSelf)
                targetSlot.DeActiveObj();
            if (isInteractableAfterPlacement)
                coll2D.enabled = true;
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
    }


    private void UpdateSpriteToPlaced()
    {
        if (sprPlaced == null) return;
        spriteRenderer.sprite = sprPlaced;
    }
    protected void OnFailSnap()
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
        var posY = transform.localPosition.y;
        idleTween = transform.DOLocalMoveY(posY + 0.3f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
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
        if (slotsSnap != null && slotsSnap.Count > 0)
            isAvailableForHint = slotsSnap.All(slot => slot != null && slot.IsReadyToReceiveItem());
        if (sprOriginal == null || sprAnim == null)
            isInteractableAfterPlacement = false;
        else
            isInteractableAfterPlacement = true;
    }

    #endregion
}