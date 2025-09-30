using EventDispatcher;
using UnityEngine;

public class BoxGameBase : MonoBehaviour
{

    public void Init()
    {
        
    }
    public void OnBoxClicked()
    {
        this.PostEvent(EventID.TAKE_OUT_ITEM);
    }
}