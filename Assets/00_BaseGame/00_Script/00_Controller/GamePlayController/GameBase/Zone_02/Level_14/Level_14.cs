using Cysharp.Threading.Tasks;
using UnityEngine;

public class Level_14 : LevelBase
{
    public Level_14_Phase2 level14Phase2;
    public int totalProgressAllPhase;
    public int GetTotalItemRequired() => totalItemsRequired;

    public override void Init()
    {
        base.Init();
        totalProgressAllPhase = totalItemsRequired + level14Phase2.GetTotalItemsRequired();
        level14Phase2.gameObject.SetActive(false);
    }

    protected override ItemSlot CreateItemSlotInstance(GameObject go)
    {
        return go.AddComponent<Slot_14>();
    }

    protected override async UniTask HandleAfterWinGame()
    {
        await UniTask.Yield();
        level14Phase2.gameObject.SetActive(true);
        level14Phase2.Init();
        GamePlayController.ResumeGame();
    }

    protected override async UniTask HandlePrevWinGame()
    {
        GamePlayController.PauseGame();
        await UniTask.Yield();
    }

    protected override void HandleFillProgress()
    {
        GamePlayController.gameScene.SetFillProgressGame(itemsPlacedCorrectly, totalProgressAllPhase);
    }
}