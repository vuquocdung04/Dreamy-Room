using DG.Tweening;
using EventDispatcher;
using UnityEngine;

public class BoxGameBase : MonoBehaviour
{
    public void ScaleToZero()
    {
        transform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
    public void OnBoxClicked()
    {
        this.PostEvent(EventID.REQUEST_TAKE_ITEM_FROM_BOX);
    }
}