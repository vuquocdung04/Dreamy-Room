using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HeartGame : MonoBehaviour
{
    [Header("Heart Settings")] public int maxHearts = 5;
    private const int RefillTimeMinutes = 30;
    private const double RefillTimeSeconds = RefillTimeMinutes * 60;
    [Header("Debug")] [SerializeField] private double timeToNextHeart;
    [SerializeField] private string debugUnlimitedEnd;

    public double GetTimeToNextHeart() => timeToNextHeart;
    public TimeSpan GetTimeToUnlimitedEnd() => timeToUnlimitedEnd;
    
    private CancellationTokenSource cts;
    private TimeSpan timeToUnlimitedEnd;

    public void Init()
    {
        cts?.Cancel();
        cts?.Dispose();
        cts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());
        CheckUnlimitedHeartExpiration();
        UpdateHeartRefill(true);
        UpdateTimers();

        HeartUpdateLoop(cts.Token).Forget();
    }

    private async UniTaskVoid HeartUpdateLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1), ignoreTimeScale: true, cancellationToken: token);

                CheckUnlimitedHeartExpiration();
                UpdateHeartRefill();
                UpdateTimers();
            }
            catch (OperationCanceledException)
            {
                Debug.Log("HeartGame: Vòng lặp UniTask đã dừng.");
                break;
            }
            catch (Exception e)
            {
                Debug.LogError($"Lỗi trong HeartUpdateLoop: {e}");
            }
        }
    }

    /// <summary>
    /// /
    /// </summary>
    private void UpdateTimers()
    {
        if (UseProfile.IsUnlimitedHeart)
        {
            timeToUnlimitedEnd = UseProfile.TimeUnlimitedHeart - DateTime.Now;
            debugUnlimitedEnd =
                $"{timeToUnlimitedEnd.Hours:D2}:{timeToUnlimitedEnd.Minutes:D2}:{timeToUnlimitedEnd.Seconds:D2}"; // Format HH:MM:SS
        }
        else
            debugUnlimitedEnd = "Không active";


        if (UseProfile.IsUnlimitedHeart || UseProfile.Heart >= maxHearts)
        {
            timeToNextHeart = 0;
            return;
        }

        TimeSpan timeSinceLastEvent = DateTime.Now - UseProfile.TimeLastOverHeart;
        timeToNextHeart = Math.Max(0, RefillTimeSeconds - timeSinceLastEvent.TotalSeconds);
    }

    private void CheckUnlimitedHeartExpiration()
    {
        if (!UseProfile.IsUnlimitedHeart) return;

        TimeSpan timeRemaining = UseProfile.TimeUnlimitedHeart - DateTime.Now;
        if (timeRemaining.TotalSeconds <= 0)
        {
            UseProfile.IsUnlimitedHeart = false;
        }
    }

    private void UpdateHeartRefill(bool isOfflineCheck = false)
    {
        if (UseProfile.Heart >= maxHearts) return;

        TimeSpan timePassed = DateTime.Now - UseProfile.TimeLastOverHeart;

        if (!isOfflineCheck && timePassed.TotalSeconds < RefillTimeSeconds) return;

        int heartsGained = (int)(timePassed.TotalSeconds / RefillTimeSeconds);
        if (heartsGained > 0)
        {
            int newHeartCount = UseProfile.Heart + heartsGained;

            if (newHeartCount >= maxHearts)
            {
                UseProfile.Heart = maxHearts;
            }
            else
            {
                UseProfile.Heart = newHeartCount;
                TimeSpan timeUsedForRefill = TimeSpan.FromSeconds(heartsGained * RefillTimeSeconds);
                UseProfile.TimeLastOverHeart = UseProfile.TimeLastOverHeart.Add(timeUsedForRefill);
            }
        }
    }

    public bool TryUseHeart()
    {
        CheckUnlimitedHeartExpiration();
        if (UseProfile.IsUnlimitedHeart)
        {
            Debug.Log("Su dung tim (UnLimitedHeart)!)");
            return true;
        }

        UpdateHeartRefill(true);
        if (UseProfile.Heart > 0)
        {
            bool wasAtMax = (UseProfile.Heart == maxHearts);
            UseProfile.Heart -= 1;

            if (wasAtMax)
                UseProfile.TimeLastOverHeart = DateTime.Now;

            return true;
        }

        Debug.Log("Khong con tim");
        return false;
    }

    public void AddUnlimitedHeart(int minutes)
    {
        DateTime newEndTime;
        if (UseProfile.IsUnlimitedHeart)
        {
            newEndTime = UseProfile.TimeUnlimitedHeart.AddMinutes(minutes);
        }
        else
        {
            newEndTime = DateTime.Now.AddMinutes(minutes);
        }

        UseProfile.IsUnlimitedHeart = true;
        UseProfile.TimeUnlimitedHeart = newEndTime;

        Debug.Log($"Đã kích hoạt {minutes} phút unlimited. Hết hạn vào: {newEndTime}");
    }
}