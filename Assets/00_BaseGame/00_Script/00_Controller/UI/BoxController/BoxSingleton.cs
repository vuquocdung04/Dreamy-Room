using System;
using UnityEngine;

public abstract class BoxSingleton<T> : BaseBox where T : BoxSingleton<T>
{
    private static T instance;

    public static T Path(string prefabPath)
    {
        if (instance == null)
        {
            T prefab = Resources.Load<T>(prefabPath);
            if (prefab == null)
            {
                Debug.LogError($"[BoxSingleton] Không tìm thấy prefab tại đường dẫn: {prefabPath}");
                return null;
            }
            instance = Instantiate(prefab);
            instance.Init();
        }

        instance.InitState();
        return instance;
    }
    
    protected abstract void Init();
    protected abstract void InitState();

    protected void RefreshLocalization(DataPlayer dataPlayer, Action callback = null)
    {
        if (dataPlayer.IsLanguageChanged)
        {
            Debug.Log("Call " + gameObject.name);
            callback?.Invoke();
        }
    }
}