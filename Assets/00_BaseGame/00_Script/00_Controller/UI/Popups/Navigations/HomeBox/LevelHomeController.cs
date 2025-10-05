using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum ItemHomeType{
    Small = 0,
    Large = 1,
}


public class LevelHomeController : MonoBehaviour
{
    [SerializeField] private Sprite sprLockSmall;
    [SerializeField] private Sprite sprLockLarge;
    [SerializeField] private Color colorLockIcon = new Color(1, 1, 1, 0.5f);

    [Header("Categories")]
    public List<LevelHomeCategory> lsCategories;

    public void Init()
    {
        var dataLevel = GameController.Instance.dataContains.dataLevel;
        foreach (var category in lsCategories)
        {
            category.Init(
                dataLevel,
                UseProfile.MaxUnlockedLevel,
                colorLockIcon,
                sprLockSmall,
                sprLockLarge,
                (item) => HandleSelection(item, delegate 
                {
                    //NOTE: GO TO LEVEL
                    Debug.Log("Play level: " + item.GetId());
                    GameController.Instance.effectChangeScene2.RunEffect(SceneName.GAME_PLAY);
                })
            );
        }
    }
    private void HandleSelection(LevelHomeItem item, System.Action callback = null)
    {
        if (item.GetId() <= UseProfile.MaxUnlockedLevel)
        {
            callback?.Invoke();
        }
        else
        {
            LevelBox.Setup().Show(); 
        }
    }
    
    //Odin
    [Button("Setup All Items In All Categories", ButtonSizes.Large)]
    private void SetupAllItems()
    {
        int currentId = 1; // ID bắt đầu từ 1
        foreach(var category in lsCategories)
        {
            category.SetupOdinItems(ref currentId);
        }
    }
}