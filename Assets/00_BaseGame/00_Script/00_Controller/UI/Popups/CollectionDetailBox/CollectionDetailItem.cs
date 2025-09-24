using UnityEngine;
using UnityEngine.UI;

public class CollectionDetailItem : MonoBehaviour
{
    [SerializeField] private int idItem;
    [SerializeField] private Image imgIcon;
    [SerializeField] private Image imgBg;
    [SerializeField] private Button btn;


    public void SetId(int id) => this.idItem = id;
    public int GetId() => idItem;

    public void Init(Sprite icon, Sprite bg, bool isActive)
    {
        imgIcon.sprite = icon;
        if (isActive)
            imgBg.sprite = bg;
        imgIcon.color = isActive ? Color.white : new Color(0, 0, 0, 0.5f);
    }
}