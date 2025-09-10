
using UnityEngine;

public enum AudioType
{
    Music,
    Sound,
    SoundBackup,
}


public class MusicController : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource musicSource;
    public AudioSource soundSource;
    public AudioSource soundSourceBackup;
    
    [Space(5)]
    [Header("Audio Clip, UI")]
    public AudioClip musicBg;
    public AudioClip clickSoundUI;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip startLevelSound;
    
    public void Init()
    {
        soundSource.volume = GameController.Instance.useProfile.OnSound ? 0.5f : 0;
        musicSource.volume = GameController.Instance.useProfile.OnMusic ? 0.5f : 0;
    }
    
    private float MusicVolume => GameController.Instance.useProfile.OnMusic ? 0.5f : 0f;
    private float SoundVolume => GameController.Instance.useProfile.OnSound ? 0.5f : 0f;

    public void PlayBgMusic()
    {
        PlaySingle(musicBg, true,AudioType.Music);
    }

    public void PlayClickSoundUI()
    {
        PlaySingle(clickSoundUI);
    }

    public void PlayWinSound()
    {
        PlaySingle(winSound);
    }

    public void PlayLoseSound()
    {
        PlaySingle(loseSound);
    }

    public void PlayStartLevelSound()
    {
        PlaySingle(startLevelSound);
    }
    public void PlaySingle(AudioClip clip, bool isLoopSound = false, AudioType audioType = AudioType.Sound)
    {
        switch (audioType)
        {
            case AudioType.Music:
                if(MusicVolume == 0) return;
                musicSource.clip = clip;
                musicSource.Play();
                break;
            case AudioType.Sound:
                if(SoundVolume == 0) return;
                soundSource.clip = clip;
                soundSource.loop = isLoopSound;
                soundSource.Play();
                break;
            case AudioType.SoundBackup:
                if(SoundVolume == 0) return;
                soundSourceBackup.clip = clip;
                soundSourceBackup.loop = isLoopSound;
                soundSourceBackup.Play();
                break;
        }
    }
    
    public void PlayMultiple(AudioClip clip, float? volumeScale = null)
    {
        if (clip == null) return;
        if (SoundVolume == 0) return;
    
        float actualVolume = volumeScale ?? (SoundVolume / 2f);
        soundSource.PlayOneShot(clip, actualVolume);
    }
    
    public void SetSoundVolume(float volume)
    {
        soundSource.volume = volume;
    }

    public void SetMusicVolume(float volume)
    {
      musicSource.volume = volume;   
    }
    
}
