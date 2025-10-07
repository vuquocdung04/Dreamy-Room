using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartLoading : MonoBehaviour
{
    public float durationLoading = 2f;
    public Image fillLoadingBar;
    public RectTransform iconLogoRect;

    private Tween iconTween;

    public void Init()
    {
        this.fillLoadingBar.fillAmount = 0;
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        yield return null;
        var sceneName = UseProfile.HasCompletedLevelTutorial ? SceneName.HOME_SCENE : SceneName.GAME_PLAY;
        var asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        yield return null;
        bool tweenComplete = false;
        fillLoadingBar.DOFillAmount(1f, durationLoading).SetEase(Ease.InOutBack)
            .OnComplete(() => tweenComplete = true);

        iconTween = IconTween();

        asyncOperation!.allowSceneActivation = false;

        while (asyncOperation.progress < 0.9f || !tweenComplete)
        {
            yield return null;
        }

        asyncOperation.allowSceneActivation = true;

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }

    private Tween IconTween()
    {
        // 2. Animation YOYO cho icon
        float startY = iconLogoRect.anchoredPosition.y;
        return iconLogoRect.DOAnchorPosY(startY + 40f, 0.2f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy()
    {
        iconTween.Kill();
        iconTween = null;
    }
}