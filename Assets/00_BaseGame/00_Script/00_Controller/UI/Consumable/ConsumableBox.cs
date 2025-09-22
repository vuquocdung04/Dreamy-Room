using System;
using EventDispatcher;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtHeart;
    [SerializeField] private TextMeshProUGUI txtCoin;
    [SerializeField] private TextMeshProUGUI txtStar;
    
    public void Init()
    {
        UpdateTextCoin();
        this.RegisterListener(EventID.CHANGE_COIN, UpdateTextCoin);
        this.RegisterListener(EventID.CHANGE_STAR, UpdateTextStar);
        this.RegisterListener(EventID.CHANGE_HEART, UpdateTextHeart);
    }
    
    private void UpdateTextCoin(object obj = null)
    {
        txtCoin.text = UseProfile.Coin.ToString();
    }

    private void UpdateTextHeart(object obj = null)
    {
        txtHeart.text = UseProfile.Heart.ToString();
    }

    private void UpdateTextStar(object obj = null)
    {
        txtStar.text = UseProfile.Star.ToString();
    }
    

    private void OnDestroy()
    {
        this.RemoveListener(EventID.CHANGE_COIN, UpdateTextCoin);
        this.RemoveListener(EventID.CHANGE_STAR, UpdateTextStar);
        this.RemoveListener(EventID.CHANGE_HEART, UpdateTextHeart);
    }
}