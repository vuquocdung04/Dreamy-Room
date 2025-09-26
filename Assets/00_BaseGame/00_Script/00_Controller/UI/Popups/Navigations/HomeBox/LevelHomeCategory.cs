using System.Collections.Generic;
using UnityEngine;

public class LevelHomeCategory : MonoBehaviour
{
    public List<LevelHomeItem> lsItems;

    public void Init(DataLevelSO dataLevel, int maxUnlockedLevel, Color colorLock, Sprite spriteLockBgSmall, Sprite spriteLockBgLarge, System.Action<LevelHomeItem> onItemClick)
    {
        foreach (var item in lsItems)
        {
            item.gameObject.SetActive(true);
            var spriteIcon = dataLevel.GetLevelSpriteById(item.GetId());
            item.InitInfo(spriteIcon);
            if (item.GetId() <= maxUnlockedLevel)
            {
                if(item.GetId() == maxUnlockedLevel)
                    item.ActiveHighlight();
                SetBgItem(item,colorLock, spriteLockBgSmall, spriteLockBgLarge);
            }
            else
            {
                SetBgItem(item,colorLock, spriteLockBgSmall, spriteLockBgLarge,true);
            }

            item.AddClickListener(onItemClick);
        }
    }

    private void SetBgItem(LevelHomeItem item, Color colorLock, Sprite sprLockSmall, Sprite sprLockLarge, bool isLock = false)
    {
        switch (item.GetItemHomeType())
        {
            case ItemHomeType.Small:
                item.UpdateState(colorLock,sprLockSmall,isLock);
                break;
            case ItemHomeType.Large:
                item.UpdateState(colorLock, sprLockLarge,isLock);
                break;
        }
    }
    public void SetupOdinItems(ref int startId)
    {
        for (int i = 0; i < lsItems.Count; i++)
        {
            lsItems[i].SetId(startId);
            lsItems[i].SetupOdin();
            if(i == lsItems.Count - 1) lsItems[i].SetItemHomeType(ItemHomeType.Large);
            startId++;
        }
    }
}