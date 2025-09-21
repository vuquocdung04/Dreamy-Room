using UnityEngine;

public class ClamSuccessBox : BoxSingleton<ClamSuccessBox>
{
    public static ClamSuccessBox Setup()
    {
        return Path(PathPrefabs.CLAIM_SUCCESS_BOX);
    }
    protected override void Init()
    {
    }

    protected override void InitState()
    {
    }
}