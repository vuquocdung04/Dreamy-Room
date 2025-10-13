using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ItemSlot : MonoBehaviour
{
    public bool isFullSlot;
    public bool isReadyShow = true;
    [SerializeField] private int indexOrder;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public List<ItemSlot> conditionSlots = new();
    
    public bool HasSpriteRenderer() => spriteRenderer;
    public int SetOrderItemPlaced() => indexOrder;
    public void Init()
    {
        isReadyShow = conditionSlots.Count == 0;
    }

    public void ValidateReadyState()
    {
        bool allConditionsMet = conditionSlots.All(slot => slot.isFullSlot);
        if (allConditionsMet)
        {
            isReadyShow = true;
        }
    }
    
    public void Active()
    {
        gameObject.SetActive(true);
    }

    public void DeActive()
    {
        gameObject.SetActive(false);
    }

    public void SetupOdin(int index, SpriteRenderer newSpireRenderer, Sprite itemSpr)
    {
        spriteRenderer = newSpireRenderer;
        if (spriteRenderer == null) return;
        indexOrder = index;
        spriteRenderer.sortingOrder = indexOrder;
        spriteRenderer.sprite = itemSpr;
        spriteRenderer.color = new Color(0, 0, 0, 0.3f);
    }
    
}