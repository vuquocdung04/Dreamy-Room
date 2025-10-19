using DG.Tweening;
using UnityEngine;

public class ItemRemover_5_Broom : ItemRemoverBase
{
    [SerializeField] private Collider2D coll;
    private Level_5 currentLevel;
    public override void Init()
    {
        currentLevel = (Level_5)GamePlayController.Instance.levelController.currentLevel;
        base.Init();
    }

    protected override void OnAllStagesComplete()
    {
        coll.enabled = false;
        currentLevel.CleanDirtCompleted();
        transform.DOScale(Vector3.zero, 0.5f).OnComplete(delegate
        {
            gameObject.SetActive(false);
        });
    }
}