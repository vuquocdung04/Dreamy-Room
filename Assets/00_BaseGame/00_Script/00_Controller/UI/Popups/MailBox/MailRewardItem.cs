using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MailRewardItem : MonoBehaviour
{
    [SerializeField] private Image imgItem;
    [SerializeField] private TextMeshProUGUI txtAmount;
    [SerializeField] private Transform checkMark;

    public void Init(Sprite sprite,int amount,bool isClaimed = false)
    {
        imgItem.sprite = sprite;
        txtAmount.text = $"x{amount}";
        checkMark.gameObject.SetActive(isClaimed);
    }
    
}