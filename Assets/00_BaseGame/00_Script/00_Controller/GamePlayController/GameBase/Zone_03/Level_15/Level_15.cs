using System;
using System.Collections.Generic;
using System.IO.Pipes;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level_15 : LevelBase
{
    public FireWork_15 fireWork;
    public List<Sprite> lsFireWorks;
    protected override ItemSlot CreateItemSlotInstance(GameObject go)
    {
        return go.AddComponent<Slot_15>();
    }

    protected override async UniTask OnBeforeWinCompleted()
    {
        await base.OnBeforeWinCompleted();
        await HandleSpawnFireWork();
    }

    private async UniTask HandleSpawnFireWork()
    {
        var left = GamePlayController.playerContains.left.position.x;
        var right = GamePlayController.playerContains.right.position.x;
        for (int i = 0 ; i < 10; i++)
        {
            float randScale = Random.Range(1f, 2f);
            float randX = Random.Range(left, right);
            float randY = Random.Range(6.5f, 10f);
            var randList = Random.Range(0, lsFireWorks.Count);
            var randSp = lsFireWorks[randList];
            
            var fireWorkClone = SimplePool2.Spawn(fireWork, new Vector2(randX,randY), Quaternion.identity);
            fireWorkClone.HandleAction(randSp, randScale);
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }
    }
    
    
}