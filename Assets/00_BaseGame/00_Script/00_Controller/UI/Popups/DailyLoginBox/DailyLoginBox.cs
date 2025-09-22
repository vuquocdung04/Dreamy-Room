using System.Collections.Generic;
using EventDispatcher;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class DailyLoginBox : BoxSingleton<DailyLoginBox>
{
    public static DailyLoginBox Setup()
    {
        return Path(PathPrefabs.DAILY_LOGIN_BOX);
    }

    public Sprite claimedSprite;
    public Sprite claimableSprite;
    public Button btnClose;
    public Button btnClaim;
    public List<DailyLoginItem> lsItems = new();

    protected override void Init()
    {
        UpdateState();

        btnClose.onClick.AddListener(Close);
        btnClaim.onClick.AddListener(delegate { OnClaim(); });
    }

    protected override void InitState()
    {
    }

    private void UpdateState()
    {
        Debug.LogError("DailyLoginBox");
        UpdateClaimButtonState();

        bool canClaimToDay = GameController.Instance.dataContains.dataDaily.HasClaimStreakToday();
        int nextDayToClaimIndex = GameController.Instance.dataContains.dataDaily.GetStreakDayIndex();

        int lastItemIndex = lsItems.Count - 1;

        for (int i = 0; i < lsItems.Count; i++)
        {
            DailyLoginItem item = lsItems[i];

            if (i < nextDayToClaimIndex)
            {
                if (i != lastItemIndex)
                    item.UpdateBackground(claimedSprite);
                item.SetAsClaimed();
            }
            else if (i == nextDayToClaimIndex && !canClaimToDay)
            {
                if (i != lastItemIndex)
                    item.UpdateBackground(claimableSprite);
                item.SetAsClaimable();
            }
            
        }
    }


    private void OnClaim()
    {
        GameController.Instance.dataContains.dataDaily.HandleStreakClaimed();
        int lastItem = lsItems.Count - 1;
        // 2. Cập nhật chỉ item vừa nhận
        int claimedDayIndex = GameController.Instance.dataContains.dataDaily.GetStreakDayIndex() - 1;
        if (claimedDayIndex >= 0 && claimedDayIndex < lsItems.Count)
        {
            if (claimedDayIndex != lastItem)
                lsItems[claimedDayIndex].UpdateBackground(claimedSprite);
            lsItems[claimedDayIndex].SetAsClaimed();
        }

        // 3. Gọi hàm chung để cập nhật trạng thái nút Claim
        UpdateClaimButtonState();
        this.PostEvent(EventID.UPDATE_NOTIFY_DAILYLOGIN);
    }

    private void UpdateClaimButtonState()
    {
        bool canClaimToday = !GameController.Instance.dataContains.dataDaily.HasClaimStreakToday();
        btnClaim.interactable = canClaimToday;
    }

    [Button("Setup Item", ButtonSizes.Large)]
    void SetupItem()
    {
        foreach (var item in this.lsItems)
        {
            item.SetupOdin();
        }
    }
}