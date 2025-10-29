using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Level_12 : LevelBase
{
    public Level_12_Phase2 level12Phase2;
    public Transform rabbit;
    public int totalProgressAllPhase;
    public int GetTotalItemRequired() => totalItemsRequired;
    public override void Init()
    {
        base.Init();
        totalProgressAllPhase = totalItemsRequired + level12Phase2.GetTotalItemsRequired();
        rabbit.gameObject.SetActive(false);
        level12Phase2.gameObject.SetActive(false);
    }

    protected override ItemSlot CreateItemSlotInstance(GameObject go)
    {
        return go.AddComponent<Slot_12>();
    }

    protected override async UniTask HandleAfterWinGame()
    {
        await UniTask.Yield();
        level12Phase2.gameObject.SetActive(true);
        level12Phase2.Init();
        GamePlayController.ResumeGame();
    }

    protected override async UniTask HandlePrevWinGame()
    {
        GamePlayController.PauseGame();
        rabbit.gameObject.SetActive(true);
        var originalScale = rabbit.localScale;
        rabbit.localScale = Vector3.zero;
        await rabbit.DOScale(originalScale, 0.5f).SetEase(Ease.OutBounce);
    }

    protected override void HandleFillProgress()
    {
        GamePlayController.gameScene.SetFillProgressGame(itemsPlacedCorrectly, totalProgressAllPhase);

    }
}