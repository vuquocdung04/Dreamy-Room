using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EffectChangeScene2 : MonoBehaviour
{
    public RectTransform rectTransform;
    public Image imgBg;
    public Image imgIcon;
    public float durationFadeIn = 1f;
    public float durationFadeOut = 0.5f;
    public bool isBusy;


    public void SetImage(Sprite bg, Sprite icon)
    {
        imgBg.sprite = bg;
        imgIcon.sprite = icon;
        imgIcon.SetNativeSize();
    }

    public async void RunEffect(string sceneName)
    {
        try
        {
            rectTransform.gameObject.SetActive(true);
            isBusy = true;
            imgIcon.transform.localScale = Vector3.one * 32f;
            await imgIcon.transform.DOScale(Vector3.zero, durationFadeIn).SetEase(Ease.Linear).AsyncWaitForCompletion();
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
            await loadOperation;
            if (sceneName.Equals(SceneName.GAME_PLAY))
                GamePlayController.Instance.PauseGame();
            await imgIcon.transform.DOScale(Vector3.one * 32f, durationFadeOut).SetEase(Ease.Linear)
                .AsyncWaitForCompletion();
            isBusy = false;
            rectTransform.gameObject.SetActive(false);
            if (sceneName.Equals(SceneName.GAME_PLAY))
                GamePlayController.Instance.ResumeGame();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            isBusy = false;
        }
    }
}