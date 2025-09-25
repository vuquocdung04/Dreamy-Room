

using UnityEngine.UI;

public class ClaimSuccessBox : BoxSingleton<ClaimSuccessBox>
{
    public static ClaimSuccessBox Setup()
    {
        return Path(PathPrefabs.CLAIM_SUCCESS_BOX);
    }

    public Button btnClose;
    public Button btnCloseByPanel;
    protected override void Init()
    {
        btnClose.onClick.AddListener(Close);
        btnCloseByPanel.onClick.AddListener(Close);
    }

    protected override void InitState()
    {
    }
}