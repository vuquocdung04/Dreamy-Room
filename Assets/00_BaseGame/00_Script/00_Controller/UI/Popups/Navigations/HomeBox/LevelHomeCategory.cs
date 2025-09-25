using System.Collections.Generic;
using UnityEngine;

public class LevelHomeCategory : MonoBehaviour
{
    public List<LevelHomeItem> lsItems;

    public void Init(DataLevelSO dataLevel, int maxUnlockedLevel, Color unlockColor, Color lockIconColor,
        Color lockBgColor, System.Action<LevelHomeItem> onItemClick)
    {
        foreach (var item in lsItems)
        {
            item.gameObject.SetActive(true);
            var spriteIcon = dataLevel.GetLevelSpriteById(item.GetId());
            item.InitInfo(spriteIcon);
            if (item.GetId() <= maxUnlockedLevel)
            {
                item.UpdateState(unlockColor, unlockColor);
            }
            else
            {
                item.UpdateState(lockIconColor, lockBgColor);
            }

            item.AddClickListener(onItemClick);
        }
    }
    public void SetupOdinItems(ref int startId)
    {
        for (int i = 0; i < lsItems.Count; i++)
        {
            lsItems[i].SetId(startId);
            lsItems[i].SetupOdin();
            startId++;
        }
    }
}