using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EffectChangeScene2 : MonoBehaviour
{
    public Transform parent;
    public SpriteRenderer imgBg;
    public SpriteRenderer imgIcon;
    public SpriteMask imgMask;
    public float durationFadeIn = 1f;
    public float durationFadeOut = 0.5f;
    public bool isBusy;
    public List<Sprite> lsSpritesBg;
    private DataLevelBase dataLevel;
    public void Init()
    {
        dataLevel = GameController.Instance.dataContains.dataLevel;
    }
    private void SetImage()
    {
        int maxUnlockedLevel = UseProfile.MaxUnlockedLevel;
        var icon = dataLevel.GetLevelSpriteById(maxUnlockedLevel);
        int rand = UnityEngine.Random.Range(0, lsSpritesBg.Count);
        imgBg.sprite = lsSpritesBg[rand];
        imgIcon.sprite = icon;
        imgMask.sprite = icon;
    }

    public async void RunEffect(string sceneName)
    {
        try
        {
            parent.gameObject.SetActive(true);
            SetImage();
            isBusy = true;
            imgIcon.transform.localScale = Vector3.one * 25f;
            await imgIcon.transform.DOScale(Vector3.zero, durationFadeIn).SetEase(Ease.Linear).AsyncWaitForCompletion();
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
            await loadOperation;
            await Task.Delay(TimeSpan.FromSeconds(0.2f));
            if (sceneName.Equals(SceneName.GAME_PLAY))
                GamePlayController.Instance.PauseGame();
            await imgIcon.transform.DOScale(Vector3.one * 25f, durationFadeOut).SetEase(Ease.Linear)
                .AsyncWaitForCompletion();
            isBusy = false;
            parent.gameObject.SetActive(false);
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