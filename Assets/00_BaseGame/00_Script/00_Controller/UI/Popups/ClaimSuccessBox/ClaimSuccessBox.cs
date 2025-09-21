using UnityEngine;

public class ClaimSuccessBox : BoxSingleton<ClaimSuccessBox>
{
    public static ClaimSuccessBox Setup()
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