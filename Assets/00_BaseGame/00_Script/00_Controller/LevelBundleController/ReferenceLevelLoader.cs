using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _00_BaseGame._00_Script._00_Controller.LevelBundleController
{
    [System.Serializable]
    public struct Zone
    {
        public string zoneName;
        public List<AssetReference> levels;
    }

    public class ReferenceLevelLoader : MonoBehaviour
    {
        [SerializeField] private List<Zone> zones;

        public AssetReference GetLevelAsset(int levelNumber)
        {
            if (levelNumber < 1) return null;

            int zoneIndex = (levelNumber - 1) / 10;
            int levelInZone = (levelNumber - 1) % 10;

            if (zoneIndex >= zones.Count || levelInZone >= zones[zoneIndex].levels.Count)
            {
                return null;
            }

            return zones[zoneIndex].levels[levelInZone];
        }

        public AsyncOperationHandle<GameObject> LoadAndInstantiateLevel(int levelNumber, Transform parent = null)
        {
            AssetReference assetRef = GetLevelAsset(levelNumber);
            if (assetRef == null || !assetRef.RuntimeKeyIsValid())
            {
                return default;
            }

            return assetRef.InstantiateAsync(parent);
        }

        public AsyncOperationHandle<GameObject> LoadLevelAsset(int levelNumber)
        {
            AssetReference assetRef = GetLevelAsset(levelNumber);
            if (assetRef == null || !assetRef.RuntimeKeyIsValid())
            {
                return default;
            }

            return assetRef.LoadAssetAsync<GameObject>();
        }
    }
}