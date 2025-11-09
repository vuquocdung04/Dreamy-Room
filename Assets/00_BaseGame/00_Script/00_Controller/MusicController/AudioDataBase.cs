using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class AudioConfig
{
    [Header("Identification")] public AudioKeyType enumKey;

    [Header("Audio Data")] [SerializeField]
    private List<AudioClip> variants;

    [Header("Pitch Randomization")] [Range(0.5f, 2f)]
    [SerializeField] private float minPitch = 0.9f;

    [Range(0.5f, 2f)] [SerializeField] private float maxPitch = 1.1f;

    [Header("Voice Limiting")] public int countLimit = 5;

    public AudioClip GetRandomClip()
    {
        if (variants == null || variants.Count == 0)
            return null;
        
        int rand =  Random.Range(0, variants.Count);
        return variants[rand];
    }

    public float GetRandomPitch()
    {
        return Random.Range(minPitch, maxPitch);
    }
    
    
    //ODin
    private string GetElementLabel
    {
        get
        {
            if (enumKey == AudioKeyType.None)
            {
                return "New Audio";
            }

            int variantCount = variants?.Count ?? 0;
            return $"🔊 {enumKey} ({variantCount} variants)";
        }
    }
}

public enum AudioKeyType
{
    None = 0,
    UIClick = 5,
    PopupOpened = 6,
    PopupClosed = 7,
    
    //GamePlay
    PickUpItem = 20,
    DropItem = 21,
    BoosterUsed = 21,
    
    
    //Level
    LevelComplete = 60,
    LevelFailed = 61,
    StarCollect = 62,
    CoinCollect = 63,
    
    //Music
    MusicBg = 100,
}

[CreateAssetMenu(fileName = "AudioDataBase", menuName = "AUDIO/AudioDataBase")]
public class AudioDataBase : ScriptableObject
{
    [Header("All audio Configurations")]
    [ListDrawerSettings(
        NumberOfItemsPerPage = 18,
        ListElementLabelName = "GetElementLabel",
        ShowPaging = true,
        DraggableItems = false,
        HideAddButton = false
    )]
    public List<AudioConfig> audioConfigs = new();
}