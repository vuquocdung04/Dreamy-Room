using EventDispatcher;
using UnityEngine;

public class FxOutlineItem : MonoBehaviour
{
    public SpriteRenderer spr;

    public void Init(ItemBase item)
    {
        transform.SetParent(item.transform);
        spr.sprite = item.GetSprite();
        spr.sortingOrder = item.GetIndexLayer() - 1;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one * 1.04f;
    }
}