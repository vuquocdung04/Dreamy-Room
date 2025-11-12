using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class LevelTut : LevelBase
{
    public Transform charSleep;
    public Transform tutHand;

    [SerializeField] private ItemBase itemTut;
    private ItemBase itemGetPositionFirst;
    private Tween sleepTween;
    private Tween tutHandTween;
    private bool hasDoneStep1;
    private bool isDoneTut;
    private Camera mainCamera;

    private float top, left, bottom, right;

    public override void Init()
    {
        base.Init();
        GamePlayController.Instance.playerContains.inputManager.enabled = false;
        mainCamera = GamePlayController.Instance.playerContains.mainCamera;
        UpdateBounds();
        SleepTween();
        DelayInitBox().Forget();
    }


    private Vector3 currentMousePosition;
    private Vector3 prevMousePosition;
    private Vector3 mouseDelta;

    private bool isDragging;

    private async UniTask DelayInitBox()
    {
        try
        {
            var token = this.GetCancellationTokenOnDestroy();
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f), cancellationToken: token);
            SetActiveBox();
            Phase1TutTween();
        }
        catch (OperationCanceledException)
        {
            
        }
    }
    

    private void Update()
    {
        if (isDoneTut) return;
        currentMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        currentMousePosition.z = 0;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(currentMousePosition, Vector2.zero);
            prevMousePosition = currentMousePosition;
            if (!hit.collider) return;
            if (!hasDoneStep1)
            {
                var box = hit.collider.GetComponent<BoxGameBase>();
                if (!box) return;
                box.OnBoxClicked(delegate
                {
                    hasDoneStep1 = true;
                    itemGetPositionFirst = itemsOutOfBox[0];
                    Phase2TutTween().Forget();
                });
            }
            else
            {
                var item = hit.collider.GetComponent<ItemBase>();
                if (!item) return;
                itemTut = item;
                tutHandTween.Kill();
                tutHandTween = null;
                tutHand.gameObject.SetActive(false);
                item.OnStartDrag(top, currentMousePosition);
                isDragging = true;
            }
        }

        if (isDragging && itemTut != null)
        {
            mouseDelta = currentMousePosition - prevMousePosition;
            itemTut.OnDrag(mouseDelta, left, right, bottom, top);
            prevMousePosition = currentMousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (itemTut != null)
            {
                itemTut.OnEndDrag(1f);
                itemTut = null;
            }
            isDragging = false;
            CheckDoneTut();
        }
    }

    protected override async UniTask OnLevelFinished()
    {
        GamePlayController.Instance.WinGame();
        await mainCamera.DOOrthoSize(14f, 0.75f).SetEase(Ease.Linear).OnComplete(delegate
        {
            GameController.Instance.ChangeScene2(SceneName.HOME_SCENE);
            UseProfile.HasCompletedLevelTutorial = true;
        });
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
    }

    private void CheckDoneTut()
    {
        if (itemsPlacedCorrectly == 1)
        {
            isDoneTut = true;
            GamePlayController.Instance.playerContains.inputManager.enabled = true;
        }
    }

    private void Phase1TutTween()
    {
        tutHandTween = tutHand.DOMoveY(-4.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    private async UniTask Phase2TutTween()
    {
        tutHand.gameObject.SetActive(false);
        await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
        tutHand.gameObject.SetActive(true);
        var newPos = itemGetPositionFirst.transform.position;
        newPos.y -= 1f;
        tutHand.transform.position = newPos;
        tutHandTween.Kill();
        tutHandTween = null;
        tutHandTween = tutHand.DOMove(new Vector2(-2.7f, -2.52f), 1f).SetLoops(-1, LoopType.Yoyo);
    }

    private void UpdateBounds()
    {
        var playerContains = GamePlayController.Instance.playerContains;
        left = playerContains.left.transform.position.x;
        top = playerContains.top.transform.position.y;
        right = playerContains.right.transform.position.x;
        bottom = GameController.Instance.useProfile.IsRemoveAds
            ? playerContains.bottom.transform.position.y
            : playerContains.bottom.transform.position.y + 2f;
    }

    private void SleepTween()
    {
        sleepTween = charSleep.DOScale(Vector3.one * 0.7f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    private void OnDestroy()
    {
        if (sleepTween != null)
        {
            sleepTween.Kill();
            sleepTween = null;
        }
    }
}