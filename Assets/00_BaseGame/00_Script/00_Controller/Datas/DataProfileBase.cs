using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DataProfile", menuName = "DATA/DataProfile", order = 2)]
public class DataProfileBase : ScriptableObject
{
    public string userName;
    public List<ProfileConflict> lsConflictFrames;
    [Space(5)]
    public List<ProfileConflict> lsConflictAvatars;


    public Sprite GetSpriteFrameById(int id)
    {
        foreach (var frame in this.lsConflictFrames)
            if (frame.id == id)
                return frame.iconSprite;
        return null;
    }

    public Sprite GetSpriteAvatarById(int id)
    {
        foreach(var avatar in this.lsConflictAvatars)
            if(avatar.id == id) 
                return avatar.iconSprite;
        return null;
    }
    
}

[System.Serializable]
public class ProfileConflict
{
    [HorizontalGroup("Data", Width = 120)] 
    public int id;
    [HorizontalGroup("Data")] 
    [PreviewField(50)]
    [HideLabel]
    public Sprite iconSprite;
}
