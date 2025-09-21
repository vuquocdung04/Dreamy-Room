using UnityEngine;

public class EditProfileBox : BoxSingleton<EditProfileBox>
{
    public static EditProfileBox Setup()
    {
        return Path(PathPrefabs.EDIT_PROFILE_BOX);
    }
    protected override void Init()
    {
    }

    protected override void InitState()
    {
    }
}