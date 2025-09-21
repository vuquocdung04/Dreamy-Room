using UnityEngine;

public class MailBox : BoxSingleton<MailBox>
{
    public static MailBox Setup()
    {
        return Path(PathPrefabs.MAIL_BOX);
    }
    protected override void Init()
    {
    }

    protected override void InitState()
    {
    }
}