using UnityEngine;

public class ItemBase : MonoBehaviour
{
    
    [SerializeField] protected Collider2D coll2D;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Sprite sprOriginal;
    [SerializeField] protected Sprite sprAnim;
    
    public virtual bool IsItemTrueSlot() => false;

    public void StartJumping(float duration)
    {
        
    }

    public virtual void OnEndDrag()
    {
        
    }

    public virtual void OnStartDrag()
    {
        
    }

    public virtual void OnDrag(Vector3 mouseWorldPosition)
    {
        
    }

    protected virtual ItemSlot GetSlotToSort()
    {
        return null;
    }

    protected virtual ItemSlot GetSlotToShowShadow()
    {
        return null;
    }

    protected virtual ItemSlot GetConditionSlost()
    {
        return null;
    }

    protected virtual ItemSlot GetConditionSlotsShadow()
    {
        return null;
    }

}