using UnityEngine;

public abstract class BoxSingleton<T> : BaseBox where T : BoxSingleton<T>
{
    private static T instance;

    public static T Setup(string prefabPath)
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
}