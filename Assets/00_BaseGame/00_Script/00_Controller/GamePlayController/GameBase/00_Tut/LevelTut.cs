using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class LevelTut : LevelBase
{
    public Transform charSleep;
    public Transform tutHand;

    [SerializeField] private ItemBase itemTut;
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
        StartTween();
        Phase1TutTween();
    }


    private Vector3 currentMousePosition;
    private Vector3 prevMousePosition;
    private Vector3 mouseDelta;

    private bool isDragging;

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
                box.OnBoxClicked();
                hasDoneStep1 = true;
                itemTut = itemsOutOfBox[0];
                Phase2TutTween().Forget();
            }
            else
            {
                tutHandTween.Kill();
                itemTut.OnStartDrag(top, currentMousePosition);
                isDragging = true;
            }
        }

        if (isDragging)
        {
            mouseDelta = currentMousePosition - prevMousePosition;
            itemTut.OnDrag(mouseDelta, left, right, bottom, top);
            prevMousePosition = currentMousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(!itemTut) return;
            itemTut.OnEndDrag(1f);
            isDragging = false;
            isDoneTut = true;
            GamePlayController.Instance.playerContains.inputManager.enabled = true;
        }
    }
    
    private void Phase1TutTween()
    {
        tutHandTween = tutHand.DOMoveY(-4.5f,0.5f).SetLoops(-1, LoopType.Yoyo);
    }
    
    private async UniTask Phase2TutTween()
    {
        tutHandTween.Kill();
        tutHand.gameObject.SetActive(false);
        await UniTask.Delay(System.TimeSpan.FromSeconds(0.3f));
        tutHand.gameObject.SetActive(true);
        tutHand.transform.position = itemTut.transform.position;
        tutHandTween = tutHand.DOMove(new Vector2(-2.7f,-2.52f),1f).SetLoops(-1, LoopType.Yoyo);
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

    private void StartTween()
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