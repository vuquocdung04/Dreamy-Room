
using System;
using UnityEngine;

public class UseProfile : MonoBehaviour
{
    public static int TargetFrameRate
    {
        get => PlayerPrefs.GetInt(StringHelper.TARGET_FRAME_RATE, 90);
        set
        {
            PlayerPrefs.SetInt(StringHelper.TARGET_FRAME_RATE, value);
            PlayerPrefs.Save();
        }
    }
 
    // Profile

    public static string UserName
    {
        get => PlayerPrefs.GetString(StringHelper.USER_NAME,"HNUE");
        set
        {
            PlayerPrefs.SetString(StringHelper.USER_NAME, value);
            PlayerPrefs.Save();
        }
    }
    
    public static int ProfileFrameDetail
    {
        get => PlayerPrefs.GetInt(StringHelper.FRAME_DETAIL, 0);
        set
        {
            PlayerPrefs.SetInt(StringHelper.FRAME_DETAIL, value);
            PlayerPrefs.Save();
        }
    }

    public static int ProfileAvatarDetail
    {
        get => PlayerPrefs.GetInt(StringHelper.AVATAR_DETAIL, 0);
        set
        {
            PlayerPrefs.SetInt(StringHelper.AVATAR_DETAIL, value);
            PlayerPrefs.Save();
        }
    }
    
    
    public static int MaxUnlockedLevel
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.MAX_UNLOCK_LEVEL,1);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.MAX_UNLOCK_LEVEL, value);
            PlayerPrefs.Save();
        }
    }
    
    public static int CurrentLevel
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.CURRENT_LEVEL, 1);
        }
        set
        {
            PlayerPrefs.SetInt (StringHelper.CURRENT_LEVEL, value);
            PlayerPrefs.Save();
        }
    }

    public bool OnSound
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.ONOFF_SOUND,1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.ONOFF_SOUND, value ? 1 : 0);
            GameController.Instance.musicController.SetSoundVolume(value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public bool OnMusic
    {
        get => PlayerPrefs.GetInt(StringHelper.ONOFF_MUSIC,1) == 1;
        set
        {
            PlayerPrefs.SetInt(StringHelper.ONOFF_MUSIC, value ? 1 : 0);
            GameController.Instance.musicController.SetMusicVolume(value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public bool OnVib
    {
        get => PlayerPrefs.GetInt(StringHelper.ONOFF_VIB,1) == 1;
        set
        {
            PlayerPrefs.SetInt(StringHelper.ONOFF_VIB, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    
    public bool IsRemoveAds
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.REMOVE_ADS,0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.REMOVE_ADS, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static int Coin
    {
        get => PlayerPrefs.GetInt(StringHelper.COIN, 0);
        set
        {
            PlayerPrefs.SetInt(StringHelper.COIN, value);
            PlayerPrefs.Save();
        }
    }

    public static int Heart
    {
        get =>  PlayerPrefs.GetInt(StringHelper.HEART, 5);
        set
        {
            PlayerPrefs.SetInt(StringHelper.HEART, value);
            PlayerPrefs.Save();
        }
    }

    public static int Star
    {
        get => PlayerPrefs.GetInt(StringHelper.STAR, 0);
        set
        {
            PlayerPrefs.SetInt(StringHelper.STAR, value);
            PlayerPrefs.Save();
        }
    }
    

    public static DateTime TimeUnlimitedHeart
    {
        get
        {
            if (PlayerPrefs.HasKey(StringHelper.TIME_UNLIMITER_HEART))
            {
                var temp = Convert.ToInt64(PlayerPrefs.GetString(StringHelper.TIME_UNLIMITER_HEART));
                return DateTime.FromBinary(temp);
            }
            else
            {
                var newDateTime = DateTime.Now.AddDays(-1);
                PlayerPrefs.SetString(StringHelper.TIME_UNLIMITER_HEART, newDateTime.ToBinary().ToString());
                PlayerPrefs.Save();
                return newDateTime;
            }
        }
        set
        {
            PlayerPrefs.SetString(StringHelper.TIME_UNLIMITER_HEART, value.ToBinary().ToString());
            PlayerPrefs.Save();
        }
    }

    public static bool IsUnlimitedHeart
    {
        get => PlayerPrefs.GetInt(StringHelper.IS_UNLIMITER_HEART,0) == 1;
        set
        {
            PlayerPrefs.SetInt(StringHelper.IS_UNLIMITER_HEART, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static DateTime TimeLastOverHeart
    {
        get
        {
            if (PlayerPrefs.HasKey(StringHelper.TIME_LAST_OVER_HEART))
            {
                var temp = Convert.ToInt64(PlayerPrefs.GetString(StringHelper.TIME_LAST_OVER_HEART));
                return DateTime.FromBinary(temp);
            }
            else
            {
                var newDateTime = DateTime.Now;
                PlayerPrefs.SetString(StringHelper.TIME_LAST_OVER_HEART, newDateTime.ToBinary().ToString());
                PlayerPrefs.Save();
                return newDateTime;
            }
        }
        set
        {
            PlayerPrefs.SetString(StringHelper.TIME_LAST_OVER_HEART, value.ToBinary().ToString());
            PlayerPrefs.Save();
        }
    }
    
    
    public static DateTime FirstTimeOpenGame
    {
        get
        {
            if (PlayerPrefs.HasKey(StringHelper.FIRST_TIME_OPEN_GAME))
            {
                var temp = Convert.ToInt64(PlayerPrefs.GetString(StringHelper.FIRST_TIME_OPEN_GAME));
                return DateTime.FromBinary(temp);
            }
            else
            {
                var newDateTime = DateTime.Now.AddDays(-1);
                PlayerPrefs.SetString(StringHelper.FIRST_TIME_OPEN_GAME, newDateTime.ToBinary().ToString());
                PlayerPrefs.Save();
                return newDateTime;
            }
        }
        set
        {
            PlayerPrefs.SetString(StringHelper.FIRST_TIME_OPEN_GAME, value.ToBinary().ToString());
            PlayerPrefs.Save();
        }
    }
    

    #region Booster

    public static int Booster_BoxBuffer
    {
        get => PlayerPrefs.GetInt(StringHelper.BOOSTER_BOXBUFFER, 0);
        set
        {
            PlayerPrefs.SetInt(StringHelper.BOOSTER_BOXBUFFER, value);
            PlayerPrefs.Save();
        }
    }

    public static int Booster_Compass
    {
        get => PlayerPrefs.GetInt(StringHelper.BOOSTER_COMPASS, 0);
        set
        {
            PlayerPrefs.SetInt(StringHelper.BOOSTER_COMPASS, value);
            PlayerPrefs.Save();
        }
    }

    public static int Booster_FrozeTime
    {
        get => PlayerPrefs.GetInt(StringHelper.BOOSTER_FROZETIME, 0);
        set
        {
            PlayerPrefs.SetInt(StringHelper.BOOSTER_FROZETIME, value);
            PlayerPrefs.Save();
        }
    }

    public static int Booster_Hint
    {
        get => PlayerPrefs.GetInt(StringHelper.BOOSTER_HINT, 3);
        set
        {
            PlayerPrefs.SetInt(StringHelper.BOOSTER_HINT, value);
            PlayerPrefs.Save();
        }
    }

    public static int Booster_MagicWand
    {
        get => PlayerPrefs.GetInt(StringHelper.BOOSTER_MAGICWAND, 3);
        set
        {
            PlayerPrefs.SetInt(StringHelper.BOOSTER_MAGNET, value);
            PlayerPrefs.Save();
        }
    }

    public static int Booster_Maget
    {
        get => PlayerPrefs.GetInt(StringHelper.BOOSTER_MAGNET, 0);
        set
        {
            PlayerPrefs.SetInt(StringHelper.BOOSTER_MAGNET, value);
            PlayerPrefs.Save();
        }
    }

    public static int Booster_Magnifier
    {
        get => PlayerPrefs.GetInt(StringHelper.BOOSTER_MAGNIFIER, 0);
        set
        {
            PlayerPrefs.SetInt(StringHelper.BOOSTER_MAGNIFIER, value);
            PlayerPrefs.Save();
        }
    }
    
    public static int Booster_TimeBuffer
    {
        get => PlayerPrefs.GetInt(StringHelper.BOOSTER_TIMEBUFFER, 0);
        set
        {
            PlayerPrefs.SetInt(StringHelper.BOOSTER_TIMEBUFFER, value);
            PlayerPrefs.Save();
        }
    }

    public static int Booster_X2Star
    {
        get => PlayerPrefs.GetInt(StringHelper.BOOSTER_X2STAR, 0);
        set
        {
            PlayerPrefs.SetInt(StringHelper.BOOSTER_X2STAR, value);
            PlayerPrefs.Save();
        }
    }
    

    #endregion
}