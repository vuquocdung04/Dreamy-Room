using System.Collections.Generic;
using UnityEngine.UI;

public class MailBox : BoxSingleton<MailBox>
{
    public static MailBox Setup()
    {
        return Path(PathPrefabs.MAIL_BOX);
    }

    public Button btnClose;
    public List<MailRewardCollection>  lsCollections;
    
    protected override void Init()
    {
    }

    protected override void InitState()
    {
    }
}