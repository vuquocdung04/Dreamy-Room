
using System;
using System.Diagnostics;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TestAsync : MonoBehaviour
{
    private async UniTask Awake()
    {
        //MyFunc().Forget();
        //await UniTask.WhenAll(MyFunc(), Pool());
        //await UniTask.WhenAll(MyFunc(), Pool2());
        await UniTask.WhenAll(MyFunc2(), MyFunc(), Pool());
        
        Debug.Log("End Awake");
    }
    private async UniTask MyFunc()
    {
        Debug.Log("MyFunc");
        Debug.Log("Delay 5f");
        await UniTask.Delay(TimeSpan.FromSeconds(5));
        Debug.Log("Async End");
    }

    private async UniTask MyFunc2()
    {
        Debug.Log("MyFunc2");
        await UniTask.Yield();
    }
    
    private async UniTask Pool()
    {
        Debug.Log("Pool: Chuẩn bị giao việc cho luồng phụ...");
        await UniTask.RunOnThreadPool(() =>
        {
            Console.WriteLine("Start Pool (from ThreadPool)");
            //System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2f));
        });
        Debug.Log("Pool: Luồng phụ đã làm xong! Giờ log ra Unity Console.");
    }

    private async UniTask Pool2()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
    }
    
}
