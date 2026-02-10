using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class AudioManager : Singleton<AudioManager>
{
    private string backgroundMusicName = "gameBGM2";
    private List<AudioSource> soundEffectSources = new List<AudioSource>();
    private AudioSource backgroundMusicSource;
    private Queue<AudioSource> availableEffectSources = new Queue<AudioSource>();
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    private bool isBackgroundMusicEnabled = true;
    private float backgroundMusicVolume = 0.5f;
    private bool isSoundEffectsEnabled = true;
    private float soundEffectsVolume = 0.5f;

    private const string MUSIC_ENABLE_KEY = "MusicEnable";
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_ENABLE_KEY = "SFXEnable";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    private const int MAX_EFFECT_SOURCES = 15;
    private AudioSource speechSource;
    private string currentSpeechName;

    protected override void Init()
    {
        LoadAudioSettings();
        // 预创建音效源
        PreCreateEffectSources();
    }

    /// <summary>
    /// 预创建音效源
    /// </summary>
    private void PreCreateEffectSources()
    {
        for (int i = 0; i < MAX_EFFECT_SOURCES; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            soundEffectSources.Add(source);
            availableEffectSources.Enqueue(source);
        }
    }

    // 加载保存的音频设置
    private void LoadAudioSettings()
    {
        isBackgroundMusicEnabled = PlayerPrefs.GetInt(MUSIC_ENABLE_KEY, 1) == 1;
        backgroundMusicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.5f);
        isSoundEffectsEnabled = PlayerPrefs.GetInt(SFX_ENABLE_KEY, 1) == 1;
        soundEffectsVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0.5f);
    }

    private void SaveAudioSettings()
    {
        PlayerPrefs.SetInt(MUSIC_ENABLE_KEY, isBackgroundMusicEnabled ? 1 : 0);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, backgroundMusicVolume);
        PlayerPrefs.SetInt(SFX_ENABLE_KEY, isSoundEffectsEnabled ? 1 : 0);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, soundEffectsVolume);
        PlayerPrefs.Save();
    }

    public bool GetBackMusicCanPlay()
    {
        return isBackgroundMusicEnabled;
    }

    public void PlayBackgroundMusic(string bgmName = "bgm")
    {
        backgroundMusicName = bgmName;

        AudioClip clip;

        if (!audioClips.TryGetValue(backgroundMusicName, out clip))
        {
            if (clip == null)
            {
                ResManager.Instance.LoadAsset<AudioClip>(bgmName, (clip2) => {
                    if (clip2 != null)
                    {
                        audioClips.Add(backgroundMusicName, clip2);
                        Debug.Log("加载完成背景音效！");
                        playBc(clip2);
                    }
                    else
                    {
                        Debug.LogError("Cannot find audio clip: " + backgroundMusicName);
                        return;
                    }
                }, () => {
                    Debug.LogError("Cannot find audio clip: " + backgroundMusicName);
                });
            }
            return;
        }
        playBc(clip);
    }

    private void playBc(AudioClip clip)
    {
        if (clip != null)
        {
            if (backgroundMusicSource == null)
            {
                backgroundMusicSource = gameObject.AddComponent<AudioSource>();
                backgroundMusicSource.loop = true;
                backgroundMusicSource.playOnAwake = false;
            }

            backgroundMusicSource.Stop();
            backgroundMusicSource.clip = clip;
            backgroundMusicSource.volume = backgroundMusicVolume;
            if (isBackgroundMusicEnabled)
            {
                backgroundMusicSource.Play();
                Debug.Log("播放背景音效 ！！！！！");
            }
            else
            {
                backgroundMusicSource.Stop();
            }
        }
        else
        {
            Debug.LogError("Cannot find audio clip: " + backgroundMusicName);
        }
    }

    private void StopBackgroundMusic()
    {
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.Stop();
        }
    }

    public void PlaySoundEffect(string effectName, float volume = 1f)
    {
        if (!isSoundEffectsEnabled) return;

        // 获取可用的音效源
        AudioSource source = GetAvailableEffectSource();
        if (source == null)
        {
            Debug.LogWarning("没有可用的音效源，跳过播放: " + effectName);
            return;
        }

        AudioClip clip;

        if (!audioClips.TryGetValue(effectName, out clip))
        {
            ResManager.Instance.LoadAsset<AudioClip>(effectName, (clip2) => {
                if (clip2 != null)
                {
                    if (audioClips.ContainsKey(effectName))
                    {
                        audioClips[effectName] = clip2;
                    }
                    else
                    {
                        audioClips.Add(effectName, clip2);
                    }

                    // 再次检查音效源是否仍然可用
                    if (source != null && !source.isPlaying)
                    {
                        PlaySoundEffectWithSource(source, clip2, effectName, volume);
                    }
                }
                else
                {
                    Debug.LogWarning("Cannot find audio clip: " + effectName);
                    // 将音效源返回池中
                    ReturnEffectSourceToPool(source);
                    return;
                }
            }, () => {
                Debug.LogWarning("Cannot find audio clip: " + effectName);
                // 将音效源返回池中
                ReturnEffectSourceToPool(source);
            });
            return;
        }

        if (clip != null)
        {
            PlaySoundEffectWithSource(source, clip, effectName, volume);
        }
        else
        {
            Debug.LogError("Cannot find audio clip: " + effectName);
            // 将音效源返回池中
            ReturnEffectSourceToPool(source);
        }
    }

    /// <summary>
    /// 使用指定的音效源播放音效
    /// </summary>
    private void PlaySoundEffectWithSource(AudioSource source, AudioClip clip, string effectName, float volume)
    {
        source.clip = clip;
        source.volume = soundEffectsVolume * volume;
        source.Play();
        Debug.Log("播放特效音 ：" + effectName);

        // 启动协程监控播放状态
        StartCoroutine(MonitorSoundEffectPlayback(source, effectName));
    }

    /// <summary>
    /// 监控音效播放状态
    /// </summary>
    private IEnumerator MonitorSoundEffectPlayback(AudioSource source, string effectName)
    {
        // 等待音效播放完成
        yield return new WaitWhile(() => source.isPlaying);

        // 播放完成后将音效源返回池中
        ReturnEffectSourceToPool(source);
    }

    /// <summary>
    /// 获取可用的音效源
    /// </summary>
    private AudioSource GetAvailableEffectSource()
    {
        // 首先检查池中是否有可用的音效源
        if (availableEffectSources.Count > 0)
        {
            AudioSource source = availableEffectSources.Dequeue();
            // 确保音效源没有被销毁
            if (source != null)
            {
                return source;
            }
        }

        // 池中没有可用的音效源，尝试从正在播放的音效源中找到一个即将结束的
        foreach (AudioSource source in soundEffectSources)
        {
            if (source != null && !source.isPlaying && !availableEffectSources.Contains(source))
            {
                return source;
            }
        }

        // 如果没有找到可用的音效源，返回null
        Debug.LogWarning("没有可用的音效源，当前使用中的音效源数量: " + (soundEffectSources.Count - availableEffectSources.Count));
        return null;
    }

    /// <summary>
    /// 将音效源返回到池中
    /// </summary>
    private void ReturnEffectSourceToPool(AudioSource source)
    {
        if (source != null && !availableEffectSources.Contains(source))
        {
            source.Stop();
            source.clip = null;
            availableEffectSources.Enqueue(source);
        }
    }

    public void ToggleBackgroundMusic(bool isOn)
    {
        isBackgroundMusicEnabled = isOn;
        SaveAudioSettings();

        if (backgroundMusicSource != null)
        {
            if (isOn)
            {
                backgroundMusicSource.Play();
            }
            else
            {
                backgroundMusicSource.Stop();
            }
        }
        else
        {
            PlayBackgroundMusic();
        }
    }

    // 设置背景音乐音量
    public void SetBackgroundMusicVolume(float volume)
    {
        backgroundMusicVolume = Mathf.Clamp01(volume);
        SaveAudioSettings();

        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.volume = backgroundMusicVolume;
        }
    }

    public void ToggleSoundEffects(bool isOn)
    {
        isSoundEffectsEnabled = isOn;
        SaveAudioSettings();

        foreach (AudioSource source in soundEffectSources)
        {
            if (source != null)
            {
                source.mute = !isOn;
            }
        }
    }

    // 设置音效音量
    public void SetSoundEffectsVolume(float volume)
    {
        soundEffectsVolume = Mathf.Clamp01(volume);
        SaveAudioSettings();
    }

    // 获取当前设置的属性
    public bool IsBackgroundMusicEnabled => isBackgroundMusicEnabled;
    public float BackgroundMusicVolume => backgroundMusicVolume;
    public bool IsSoundEffectsEnabled => isSoundEffectsEnabled;
    public float SoundEffectsVolume => soundEffectsVolume;

    public void PlaySpeech(string speechName, float volume = 1f)
    {
        if (!isSoundEffectsEnabled) return;

        if (speechSource != null)
        {
            if (speechSource.isPlaying)
            {
                StopSpeech();
            }
        }

        AudioClip clip;
        if (audioClips.TryGetValue(speechName, out clip))
        {
            PlaySpeechClip(clip, volume, speechName);
        }
        else
        {
            // 加载语音资源
            ResManager.Instance.LoadAsset<AudioClip>(speechName, (clip2) => {
                if (clip2 != null)
                {
                    if (audioClips.ContainsKey(speechName))
                    {
                        audioClips[speechName] = clip2;
                    }
                    else
                    {
                        audioClips.Add(speechName, clip2);
                    }
                    PlaySpeechClip(clip2, volume, speechName);
                }
                else
                {
                    Debug.LogWarning("找不到语音资源：" + speechName);
                }
            }, () => {
                Debug.LogWarning("加载语音资源失败：" + speechName);
            });
        }
    }

    private void PlaySpeechClip(AudioClip clip, float volume, string speechName)
    {
        if (speechSource == null)
        {
            speechSource = gameObject.AddComponent<AudioSource>();
            speechSource.loop = false;
            speechSource.playOnAwake = false;
        }

        speechSource.clip = clip;
        speechSource.volume = soundEffectsVolume * volume;
        speechSource.Play();
        currentSpeechName = speechName;
    }

    /// <summary>
    /// 停止当前正在播放的人物说话语音
    /// </summary>
    public void StopSpeech()
    {
        if (speechSource == null)
        {
            return;
        }
        if (speechSource.isPlaying)
        {
            Debug.Log("停止语音：" + (currentSpeechName ?? "未知语音"));
            speechSource.Stop();
            currentSpeechName = null;
        }
    }

    /// <summary>
    /// 检查是否有语音正在播放
    /// </summary>
    /// <returns>如果有语音正在播放则返回true，否则返回false</returns>
    public bool IsSpeechPlaying()
    {
        return speechSource != null && speechSource.isPlaying;
    }

    /// <summary>
    /// 获取当前正在播放的语音名称
    /// </summary>
    /// <returns>当前语音名称，没有则返回null</returns>
    public string GetCurrentSpeechName()
    {
        return currentSpeechName;
    }


    /// <summary>
    /// 清理所有音效
    /// </summary>
    public void ClearAllSoundEffects()
    {
        foreach (AudioSource source in soundEffectSources)
        {
            if (source != null && source.isPlaying)
            {
                source.Stop();
            }
        }

        // 清空池并重新填充
        availableEffectSources.Clear();
        foreach (AudioSource source in soundEffectSources)
        {
            if (source != null)
            {
                source.clip = null;
                availableEffectSources.Enqueue(source);
            }
        }
    }

    /// <summary>
    /// 获取当前音效源使用情况
    /// </summary>
    public void LogSoundEffectUsage()
    {
        int playingCount = soundEffectSources.Count(source => source != null && source.isPlaying);
        int availableCount = availableEffectSources.Count;
        Debug.Log($"音效源使用情况: 总数量={soundEffectSources.Count}, 正在播放={playingCount}, 可用={availableCount}");
    }
}