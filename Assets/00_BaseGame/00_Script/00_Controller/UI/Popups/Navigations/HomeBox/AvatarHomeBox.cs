using System;
using EventDispatcher;
using UnityEngine;
using UnityEngine.UI;

public class AvatarHomeBox : MonoBehaviour
{
    public Image imgAvatar;
    public Image imgFrame;

    public void Init()
    {
        ChangeAvatar();
        this.RegisterListener(EventID.CHANGE_AVATAR,ChangeAvatar);
    }

    private void ChangeAvatar(object obj = null)
    {
        var dataProfile = GameController.Instance.dataContains.dataProfile;
        imgAvatar.sprite = dataProfile.GetSpriteAvatarById(UseProfile.ProfileAvatarDetail);
        imgFrame.sprite = dataProfile.GetSpriteFrameById(UseProfile.ProfileFrameDetail);
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.CHANGE_AVATAR,ChangeAvatar);
    }
}