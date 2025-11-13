using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource asBg;
    public List<AudioSource> asOthers;

    private AudioDataBase audioDataBase;
    private Dictionary<AudioKeyType, AudioConfig> audioLookup;
    private UseProfile useProfile;

    public void Init()
    {
        audioDataBase = GameController.Instance.dataContains.audioData;
        useProfile = GameController.Instance.useProfile;

        
        BuildAudioLookup();
        SetInitVolumes();
        if (UseProfile.HasCompletedLevelTutorial)
            PlayMusic(AudioKeyType.BGMHome);
    }

    private void BuildAudioLookup()
    {
        audioLookup = new();
        foreach (var config in audioDataBase.audioConfigs)
        {
            if (config.enumKey == AudioKeyType.None)
                continue;
            if (!audioLookup.ContainsKey(config.enumKey))
                audioLookup.Add(config.enumKey, config);
            else
                Debug.LogWarning($"Tìm thấy AudioKey bị trùng: {config.enumKey}");
        }
    }
    private void SetInitVolumes()
    {
        SetMusicVolume(useProfile.OnMusic ? 1f : 0f);
        SetSoundVolume(useProfile.OnSound ? 1f : 0f);
    }


    private AudioSource GetAvailableSource()
    {
        foreach (var source in asOthers)
            if (!source.isPlaying)
                return source;
        // Tìm source có thời gian còn lại ít nhất (gần hết)
        AudioSource nearestEnd = asOthers[0];
        float minTimeLeft = float.MaxValue;
    
        foreach (var source in asOthers)
        {
            if (source.clip == null) continue;
            float timeLeft = source.clip.length - source.time;
            if (timeLeft < minTimeLeft)
            {
                minTimeLeft = timeLeft;
                nearestEnd = source;
            }
        }
        return nearestEnd;
    }

    /// <summary>
    /// Phát một SFX (âm thanh ngắn) dựa trên Key.
    /// </summary>
    private Dictionary<AudioKeyType, float> lastPlayTimes = new();
    
    public void PlaySfx(AudioKeyType key)
    {
        if(!useProfile.OnSound) return;
        
        if (lastPlayTimes.TryGetValue(key, out float lastTime))
        {
            if (Time.time - lastTime < 0.1f)
                return;
        }
        lastPlayTimes[key] = Time.time;
        
        if (!audioLookup.TryGetValue(key, out var config))
        {
            Debug.LogWarning($"Không tìm thấy AudioKey: {key}");
            return;
        }

        AudioClip clipToPlay = config.GetRandomClip();
        if (clipToPlay == null) return;

        AudioSource source = GetAvailableSource();
        if (source == null) return;

        source.clip = clipToPlay;
        source.pitch = config.GetRandomPitch();
        source.Play();
    }

    public void PlayMusic(AudioKeyType key)
    {
        if(!useProfile.OnMusic) return;
        if (!audioLookup.TryGetValue(key, out AudioConfig config))
        {
            Debug.LogWarning($"Không tìm thấy AudioKey nhạc: {key}");
            return;
        }

        AudioClip clipToPlay = config.GetRandomClip();
        if (clipToPlay == null) return;

        asBg.clip = clipToPlay;
        asBg.loop = true;
        asBg.pitch = 1f;
        asBg.Play();
    }

    public void StopMusic()
    {
        asBg.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        asBg.volume = volume;
    }

    public void SetSoundVolume(float volume)
    {
        foreach (var source in asOthers)
        {
            source.volume = volume;
        }
    }
}