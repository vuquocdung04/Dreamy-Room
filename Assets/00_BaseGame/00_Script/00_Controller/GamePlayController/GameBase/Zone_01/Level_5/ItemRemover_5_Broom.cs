using DG.Tweening;
using UnityEngine;

public class ItemRemover_5_Broom : ItemRemoverBase
{
    private Level_5 currentLevel;
    public override void Init()
    {
        currentLevel = (Level_5)GamePlayController.Instance.levelController.currentLevel;
        base.Init();
    }

    protected override void OnAllStagesComplete()
    {
        currentLevel.CleanDirtCompleted();
        
        transform.DOScale(Vector3.zero, 0.5f);
        gameObject.SetActive(false);
    }
}