using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using EventDispatcher;
using UnityEngine;

public class BoxGameBase : MonoBehaviour
{
    [SerializeField] private Collider2D coll;
    [SerializeField] private Color boxColor;
    [SerializeField] private float lidFrameDuration = 0.1f;
    [SerializeField] private float tapeFrameDuration = 0.1f;
    
    [Space(5)]
    [SerializeField] private SpriteRenderer lid0SpriteRenderer;
    [SerializeField] private Sprite originalSpriteLid0;
    [SerializeField] private SpriteRenderer lid1SpriteRenderer;
    [SerializeField]  private Sprite originalSpriteLid1;
    [SerializeField] private SpriteRenderer tapeSpriteRenderer;

    [SerializeField] private List<Sprite> framesLid0;
    [SerializeField] private List<Sprite> framesLid1;
    [SerializeField] private List<Sprite> framesTape;
    private Vector3 originalScale;
    private bool hasBoxOpened;
    public bool HasBoxOpened() => hasBoxOpened;
    
    public void Start()
    {
        originalScale = transform.localScale;
    }
    
    
    public void ScaleToZero()
    {
        coll.enabled = false;
        transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => { gameObject.SetActive(false); });
    }

    public void OnBoxClicked()
    {
        this.PostEvent(EventID.REQUEST_TAKE_ITEM_FROM_BOX);
        if (!hasBoxOpened)
            PlayAnimationFirstClick().Forget();
    }

    public void CloseBox()
    {
        lid0SpriteRenderer.sprite = originalSpriteLid0;
        lid0SpriteRenderer.gameObject.SetActive(true);
        lid1SpriteRenderer.sprite =  originalSpriteLid1;
        lid1SpriteRenderer.gameObject.SetActive(false);
    }

    public void PlayAnimation()
    {
        // Tạo sequence để chạy các animation liên tiếp
        Sequence sequence = DOTween.Sequence();
    
        // Bước 1: Scale Y nhỏ lại, X to ra (squash)
        sequence.Append(transform.DOScale(new Vector3(1.3f, 0.7f, 1f), 0.1f).SetEase(Ease.OutQuad));
    
        // Bước 2: Scale Y to ra, X nhỏ lại (stretch) 
        sequence.Append(transform.DOScale(new Vector3(1f, 0.9f, 1f), 0.15f).SetEase(Ease.OutQuad));
    
        // Bước 3: Về lại bình thường với bounce nhẹ
        sequence.Append(transform.DOScale(originalScale, 0.2f).SetEase(Ease.OutBack, 1.2f));
    }

    private async UniTaskVoid PlayAnimationFirstClick()
    {
        await PlayFrameAnimation(framesTape, tapeSpriteRenderer, tapeFrameDuration,true);

        await PlayFrameAnimation(framesLid0, lid0SpriteRenderer, lidFrameDuration);
        lid0SpriteRenderer.sprite = originalSpriteLid0;
        lid0SpriteRenderer.gameObject.SetActive(false);
        
        lid1SpriteRenderer.gameObject.SetActive(true);
        await PlayFrameAnimation(framesLid1, lid1SpriteRenderer, lidFrameDuration);

        hasBoxOpened = true;
    }

    private async UniTask PlayFrameAnimation(List<Sprite> frames, SpriteRenderer spriteRenderer, float delaySeconds,
        bool setActiveFalseAfter = false)
    {
        foreach (var frame in frames)
        {
            spriteRenderer.sprite = frame;
            await UniTask.Delay(System.TimeSpan.FromSeconds(delaySeconds));
        }

        if (setActiveFalseAfter)
            spriteRenderer.gameObject.SetActive(false);
    }
}