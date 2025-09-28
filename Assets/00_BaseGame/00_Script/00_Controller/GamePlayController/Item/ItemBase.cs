using UnityEngine;

public class ItemBase : MonoBehaviour
{
    [SerializeField] protected ItemState itemState;
    [SerializeField] protected AudioClip sfxPickUp;
    [SerializeField] protected AudioClip sfxDrop;

    protected Vector3 velocity;


    public virtual bool IsItemTrueSlot() => false;

    public virtual void OnEndDrag()
    {
        
    }

    public virtual void OnStartDrag()
    {
        
    }

    public virtual void OnDrag(Vector3 mouseWorldPosition)
    {
        
    }
    

}