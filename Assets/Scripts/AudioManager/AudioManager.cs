using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public enum AudioType
    {
        MusicAudio,
        SfxAudio
    }
    
    // private instance
    private static AudioManager _instance;
    // instance getter
    public static AudioManager Instance => _instance;

    [Header("Music Audio")]
    [SerializeField] private AudioSource musicSource;

    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioClip[] musicAudioClips;

    [Header("Sfx Audio")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip[] sfxAudioClips;

    private float currentMusicVolume = 1;
    private float currentSfxVolume = 1;
    private const string MusicVolume = "vol1";
    
    private void Awake()
    {
        // check if another instance for TutorialModuleManager exists
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        currentMusicVolume = PlayerPrefs.GetFloat(AudioType.MusicAudio.ToString(), 1f);
        currentSfxVolume = PlayerPrefs.GetFloat(AudioType.SfxAudio.ToString(), 1f);
        SetMusicVolume(currentMusicVolume);
        SetSfxVolume(currentSfxVolume);
    }

    #region Audio Play functions

    private void PlayMusic(string clipName)
    {
#if !UNITY_SERVER
        foreach (AudioClip clip in musicAudioClips)
        {
            if (clip.name == clipName)
            {
                StartCoroutine(PlayMusicFade(clip));
                // musicSource.clip = clip;
                // musicSource.Play();
                break;
            }
        }
#endif        
    }

    IEnumerator PlayMusicFade(AudioClip nextAudioClip)
    {
        yield return StartFade(musicMixer, MusicVolume, 0.85f, 0);
        musicSource.clip = nextAudioClip;
        musicSource.Play();
        yield return StartFade(musicMixer, MusicVolume, 0.85f, 1);
    }

    public void PlayMenuBGM()
    {
        PlayMusic("BGM_MainMenu");
    }

    public void PlayGameplayBGM()
    {
        PlayMusic("SpaceChillout");
    }

    public void PlaySfx(string clipName)
    {
        foreach (AudioClip clip in sfxAudioClips)
        {
            if (clip.name == clipName)
            {
                sfxSource.PlayOneShot(clip);
                break;
            }
        }
    }

    #endregion
    
    #region Audio Settings functions

    public float GetCurrentVolume(AudioType audioType)
    {
        switch (audioType)
        {
            case AudioType.MusicAudio:
                return currentMusicVolume;
            case AudioType.SfxAudio:
                return currentSfxVolume;
        }

        // return default value for volume (100%)
        return 1;
    }
    
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        
        // store current volume
        SetPlayerPrefsVolume(AudioType.MusicAudio.ToString(), volume);
        currentMusicVolume = volume;
    }

    public void SetSfxVolume(float volume)
    {
        sfxSource.volume = volume;
        
        // store current volume
        SetPlayerPrefsVolume(AudioType.SfxAudio.ToString(), volume);
        currentSfxVolume = volume;
    }
    
    private void SetPlayerPrefsVolume(string prefKeyName, float volume)
    {
        PlayerPrefs.SetFloat(prefKeyName, volume);
    }

    #endregion
    
    public static IEnumerator StartFade(AudioMixer audioMixer, string exposedParam, float duration, float targetVolume)
    {
        float currentTime = 0;
        float currentVol;
        audioMixer.GetFloat(exposedParam, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        yield break;
    }
}
