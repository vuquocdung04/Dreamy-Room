using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadingWithPath : MonoBehaviour
{
    private AsyncOperationHandle<GameObject> preloadHandle;
    private GameObject preloadedLevelAsset;    
    private GameObject currentLevelInstance;  

    private const string BasePath = "Assets/00_BaseGame/02_Prefabs_Sprite/Prefabs/";
    
    public void PreloadLevelAsset(int levelNumber, System.Action<bool> onComplete = null)
    {
        // Giải phóng asset cũ trước khi load asset mới để tránh rò rỉ bộ nhớ
        UnloadPreloadedAsset(); 
        try
        {
            string levelAddress = GetLevelAddress(levelNumber);
            
            if (string.IsNullOrEmpty(levelAddress))
            {
                Debug.LogError($"Invalid level number for preload: {levelNumber}");
                onComplete?.Invoke(false);
                return;
            }

            Debug.Log($"Preloading level asset: {levelAddress}");
            
            preloadHandle = Addressables.LoadAssetAsync<GameObject>(levelAddress);
            
            preloadHandle.Completed += (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    preloadedLevelAsset = handle.Result; // << THAY ĐỔI: Lưu lại kết quả asset đã load
                    Debug.Log($"✅ Level {levelNumber} asset preloaded successfully!");
                    onComplete?.Invoke(true);
                }
                else
                {
                    Debug.LogError($"❌ Failed to preload level {levelNumber} asset");
                    onComplete?.Invoke(false);
                }
            };
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error preloading level {levelNumber}: {ex.Message}");
            onComplete?.Invoke(false);
        }
    }
    public GameObject InstantiateLevelFromPreloaded()
    {
        UnloadCurrentLevel();
        if (!preloadHandle.IsValid() || preloadHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("❌ Preload handle is invalid or not completed!");
            return null;
        }
        
        if (preloadedLevelAsset != null)
        {
            Debug.LogWarning($"Instantiating from preloaded asset: {preloadedLevelAsset.name}");
            currentLevelInstance = Instantiate(preloadedLevelAsset);
            return currentLevelInstance;
        }
        
        Debug.LogError("❌ Preloaded asset is not available! Cannot instantiate.");
        return null;
    }

    private void UnloadCurrentLevel()
    {
        // << THAY ĐỔI: Dùng Destroy thay vì ReleaseInstance
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
            currentLevelInstance = null;
            Debug.Log("Current level instance destroyed");
        }
    }

    private void UnloadPreloadedAsset()
    {
        if (preloadHandle.IsValid())
        {
            preloadedLevelAsset = null;
            Addressables.Release(preloadHandle);
            Debug.Log("Preloaded asset unloaded");
        }
    }
    
    
    private string GetLevelAddress(int levelNumber)
    {
        if (levelNumber < 1 || levelNumber > 185)
        {
            Debug.LogError($"Level number {levelNumber} is out of range (1-185)");
            return null;
        }
        if (levelNumber <= 170)
        {
            int zoneIndex = (levelNumber - 1) / 10 + 1; // Zone 1-17
            return $"{BasePath}Zone{zoneIndex}/Level_{levelNumber}.prefab";
        }
        // Level 171-185: Zone 18
        return $"{BasePath}Zone18/Level_{levelNumber}.prefab";
    }
}