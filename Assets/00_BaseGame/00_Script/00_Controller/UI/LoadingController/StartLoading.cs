using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartLoading : MonoBehaviour
{
    public Image background;
    public Image fillLoadingBar;
    public TextMeshProUGUI txtLoading;

    public void Init()
    {
        this.fillLoadingBar.fillAmount = 0;
        StartCoroutine(LoadAdsToChangeScene());
        StartCoroutine(LoadingText());
    }
    IEnumerator LoadAdsToChangeScene()
    {
        yield return new WaitForSeconds(1);

        // Bắt đầu coroutine giả lập loading bar từ 0 -> 0.9 trong 1.5s
        StartCoroutine(SimulateLoadingProgress(1.5f, () =>
        {
            // Khi xong 0.9, bắt đầu load scene thực sự
            StartCoroutine(ChangeScene());
        }));
    }

    // Hàm mô phỏng loading bar từ 0 đến 0.9 trong thời gian duration
    IEnumerator SimulateLoadingProgress(float duration, System.Action onComplete)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / duration) * 0.9f;
            fillLoadingBar.fillAmount = progress;
            yield return null;
        }

        fillLoadingBar.fillAmount = 0.9f;
        
        onComplete?.Invoke();
    }

    private IEnumerator ChangeScene()
    {
        var homeScene = SceneName.HOME_SCENE;
        var asyncOperation = SceneManager.LoadSceneAsync(homeScene, LoadSceneMode.Single);

        // Không cho scene active ngay lập tức
        asyncOperation!.allowSceneActivation = false;

        // Đợi cho đến khi tiến trình gần xong (0.9)
        while (asyncOperation.progress < 0.9f)
        {
            yield return null;
        }

        // Fill nốt progress từ 0.9 -> 1.0 trong 0.5s
        float elapsed = 0f;
        float duration = 0.5f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = 0.9f + (elapsed / duration) * 0.1f;
            fillLoadingBar.fillAmount = progress;
            yield return null;
        }

        fillLoadingBar.fillAmount = 1f;
        yield return new WaitForSeconds(0.2f);

        // Cho phép active scene
        asyncOperation.allowSceneActivation = true;
    }

    private IEnumerator LoadingText()
    {
        var wait = new WaitForSeconds(1);

        while (true)
        {
            txtLoading.text = "LOADING.";
            yield return wait;

            txtLoading.text = "LOADING..";
            yield return wait;

            txtLoading.text = "LOADING...";
            yield return wait;
        }
        // ReSharper disable once IteratorNeverReturns
    }
}
