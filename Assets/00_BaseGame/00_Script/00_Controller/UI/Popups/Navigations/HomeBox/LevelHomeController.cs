using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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

    [Header("Scale Settings")]
    [SerializeField] private float targetHeightSmall = 250f;
    [SerializeField] private float targetHeightLarge = 300f;
    [Header("Categories")]
    public List<LevelHomeCategory> lsCategories;
    [SerializeField] private int batchSize = 50; // Số item xử lý mỗi frame
    private CancellationTokenSource cts;

    public async void Init()
    {
        try
        {
            cts?.Cancel();
            cts?.Dispose();
            cts = new CancellationTokenSource();
            
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
                        UseProfile.CurrentLevel = item.GetId();
                        GameController.Instance.ChangeScene2(SceneName.GAME_PLAY);
                    })
                );
            }
        
            await ScaleAllIconsAsync(cts.Token);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Init cancelled");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void HandleSelection(LevelHomeItem item, Action callback = null)
    {
        if (item.GetId() <= UseProfile.MaxUnlockedLevel)
            callback?.Invoke();
        else
            LevelBox.Setup().Show();
    }

    private async UniTask ScaleAllIconsAsync(CancellationToken ct)
    {
        int count = 0;
        foreach (var category in lsCategories)
        {
            foreach (var item in category.lsItems)
            {
                float targetHeight = item.GetItemHomeType() == ItemHomeType.Small 
                    ? targetHeightSmall 
                    : targetHeightLarge;
                item.FitIconToTargetHeight(targetHeight);
                count++;
                
                if (count % batchSize == 0)
                    await UniTask.Yield(ct);
            }
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