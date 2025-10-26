using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GiftDataBase", menuName = "DATA/GiftDataBase", order = 1)]
public class GiftDataBase : ScriptableObject
{
    public List<GiftDataEntry> lsGiftData;

    public bool GetGift(GiftType giftType, out Gift gift)
    {
        var entry = lsGiftData.FirstOrDefault(g => g.type == giftType);

        if (entry != null)
        {
            gift = entry.giftData;
            return true;
        }

        gift = null;
        return false;
    }

    public void Claim(GiftType giftType, int amount, Reason reason = Reason.None)
    {
        switch (giftType)
        {
            case GiftType.Coin:
                UseProfile.Coin += amount;
                EventDispatcher.EventDispatcher.Instance.PostEvent(EventID.CHANGE_COIN);
                break;
            case GiftType.Heart:
                UseProfile.Heart += amount;
                break;
            case GiftType.Star:
                UseProfile.Star += amount;
                EventDispatcher.EventDispatcher.Instance.PostEvent(EventID.CHANGE_STAR);
                break;
            case GiftType.RemoveAds:
                GameController.Instance.useProfile.IsRemoveAds = true;
                break;
            case GiftType.BoosterBoxBuffet:
                UseProfile.Booster_BoxBuffer += amount;
                break;
            case GiftType.BoosterCompass:
                UseProfile.Booster_Compass += amount;
                break;
            case GiftType.BoosterFrozenTime:
                UseProfile.Booster_FrozeTime += amount;
                break;
            case GiftType.BoosterHint:
                UseProfile.Booster_Hint += amount;
                break;
            case GiftType.BoosterMagicWand:
                UseProfile.Booster_MagicWand +=  amount;
                break;
            case GiftType.BoosterMagnet:
                UseProfile.Booster_Maget += amount;
                break;
            case GiftType.BoosterMagnifier:
                UseProfile.Booster_Magnifier += amount;
                break;
            case GiftType.BoosterTimeBuffer:
                UseProfile.Booster_TimeBuffer += amount;
                break;
            case GiftType.BoosterX2Star:
                UseProfile.Booster_X2Star += amount;
                break;
        }
    }

}
[Serializable]
public class GiftDataEntry
{
    public GiftType type;
    public Gift giftData;
}
[Serializable]
public class Gift
{
    public Sprite giftSprite;
    //public GameObject giftAnim;
}
