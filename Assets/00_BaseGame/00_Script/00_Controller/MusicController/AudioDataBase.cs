using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class AudioConfig
{
    [Header("Identification")] public AudioKeyType key;

    [Header("Audio Data")] [SerializeField]
    private List<AudioClip> variants;

    [Header("Pitch Randomization")] [Range(0.5f, 2f)]
    public float minPitch = 0.9f;

    [Range(0.5f, 2f)] public float maxPitch = 1.1f;

    [Header("Voice Limiting")] public int countLimit = 5;

    public AudioClip GetRandomClip()
    {
        int rand =  Random.Range(0, variants.Count);
        return variants[rand];
    }
    
    
    //ODin
    private string GetElementLabel
    {
        get
        {
            if (key == AudioKeyType.None)
            {
                return "New Audio";
            }

            int variantCount = variants?.Count ?? 0;
            return $"🔊 {key} ({variantCount} variants)";
        }
    }
}

public enum AudioKeyType
{
    None,
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