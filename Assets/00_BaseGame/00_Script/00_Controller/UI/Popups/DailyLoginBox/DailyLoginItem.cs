using UnityEngine;
using UnityEngine.UI;

public class DailyLoginItem : MonoBehaviour
{
    [SerializeField] Image backgroundImage;
    [SerializeField] Transform rewardDisplayGroup;
    [SerializeField] Transform claimedCheckmark;
    
    
    public void UpdateBackground(Sprite bgSprite)
    {
        backgroundImage.sprite = bgSprite;
    }

    public void SetAsClaimable()
    {
        rewardDisplayGroup.gameObject.SetActive(true);
        claimedCheckmark.gameObject.SetActive(false);
    }

    public void SetAsClaimed()
    {
        rewardDisplayGroup.gameObject.SetActive(false);
        claimedCheckmark.gameObject.SetActive(true);
    }

}