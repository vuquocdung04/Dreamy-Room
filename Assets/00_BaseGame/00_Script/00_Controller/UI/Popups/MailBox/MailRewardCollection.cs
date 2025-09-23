using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MailRewardCollection : MonoBehaviour
{
    
    public Button btnShowDetail;
    public Image imgItemIcon;
    public TextMeshProUGUI txtCalender;
    public TextMeshProUGUI txtDescription;
    public List<MailRewardItem> lsItems;

    public void Init()
    {
        foreach (var item in lsItems)
        {
            
        }
    }
}