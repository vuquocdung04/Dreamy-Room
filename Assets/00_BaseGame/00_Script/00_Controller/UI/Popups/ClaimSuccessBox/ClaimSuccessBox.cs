

using UnityEngine.UI;

public class ClaimSuccessBox : BoxSingleton<ClaimSuccessBox>
{
    public static ClaimSuccessBox Setup()
    {
        return Path(PathPrefabs.CLAIM_SUCCESS_BOX);
    }

    public Button btnClose;
    protected override void Init()
    {
        btnClose.onClick.AddListener(Close);
    }

    protected override void InitState()
    {
    }
}