
using UnityEngine;

public abstract class BaseBox : MonoBehaviour
{
    [SerializeField] protected RectTransform mainPanel;
    [SerializeField] protected bool isAnim = true;

    protected virtual void OnEnable()
    {
        if(isAnim)
            mainPanel.localScale = Vector3.zero;
        DoAppear();
        OnStart();
    }

    protected virtual void DoAppear()
    {
        // Hieu ung xuat hien
    }

    protected virtual void OnStart()
    {
        // Code chay bat dau hien thi
    }


    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}
