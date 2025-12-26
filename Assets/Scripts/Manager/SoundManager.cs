using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    BGM,
    EFFECT,
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioMixer mAudioMixer;
    [SerializeField] private AudioClip[] mPreloadClips;

    private Dictionary<string, AudioClip> mClipsDictionary;

    private AudioSource bgmSource;
    private AudioSource effectSource;

    private float lastEffectVolume = 1f;
    private float lastBGMVolume = 1f;

    /// <summary>
    /// bgm은 루프 켜두고 이펙트는 루프 꺼두기
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.outputAudioMixerGroup = mAudioMixer.FindMatchingGroups("BGM")[0];


        effectSource = gameObject.AddComponent<AudioSource>();
        effectSource.loop = false;
        effectSource.outputAudioMixerGroup = mAudioMixer.FindMatchingGroups("EFFECT")[0];

        mClipsDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in mPreloadClips)
        {
            if (!mClipsDictionary.ContainsKey(clip.name))
            {
                mClipsDictionary.Add(clip.name, clip);
            }
        }
    }

    /// <summary>
    /// 클립 이름으로 오디오를 가져온다
    /// </summary>
    private AudioClip GetClip(string clipName)
    {
        AudioClip clip = null;
        mClipsDictionary.TryGetValue(clipName, out clip);

        if (clip == null)
        {
            Debug.LogError("클립이 없습니다.");
            return null;
        }
        return clip;
    }

    /// <summary>
    /// 효과음 재생
    /// </summary>
    public void PlayEffect(string clipName)
    {
        AudioClip clip = GetClip(clipName);
        if (clip == null)
        {
            return;
        }
        effectSource.PlayOneShot(clip);
    }

    /// <summary>
    /// BGM 재생
    /// </summary>
    public void PlayBGM(string clipName)
    {
        AudioClip clip = GetClip(clipName);
        if (clip == null)
        {
            return;
        }
        if (bgmSource.clip == clip && bgmSource.isPlaying)
        {
            return;
        }

        bgmSource.clip = clip;
        bgmSource.Play();
    }

    /// <summary>
    /// BGM 정지
    /// </summary>
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    /// <summary>
    /// 볼륨 설정
    /// </summary>
    public void SetVolume(SoundType type, float value)
    {
        mAudioMixer.SetFloat(type.ToString(), value);
    }

    /// <summary>
    /// 게임 시작 시 초기 볼륨 설정
    /// </summary>
    public void InitVolumes(float bgm, float effect)
    {
        SetVolume(SoundType.BGM, bgm);
        SetVolume(SoundType.EFFECT, effect);
    }

    public void SetBGMMute(bool isMute)
    {
        if (isMute)
        {
            lastBGMVolume = PlayerPrefs.GetFloat("BGM_VOL", 1f);
            mAudioMixer.SetFloat("BGM", -80f);
        }

        else
        {
            //float bgm = PlayerPrefs.GetFloat("BGM_VOL", 1f);
            float dB = Mathf.Lerp(-80f, 0f, lastBGMVolume);
            mAudioMixer.SetFloat("BGM", dB);
        }
    }
    public void SetEffectMute(bool isMute)
    {
        if (isMute)
        {
            lastEffectVolume = PlayerPrefs.GetFloat("EFFECT_VOL", 1f);
            mAudioMixer.SetFloat("EFFECT", -80f);
        }
        else
        {
            //float effect = PlayerPrefs.GetFloat("EFFECT_VOL", 1f);
            float dB = Mathf.Lerp(-80f, 0f, lastEffectVolume);
            mAudioMixer.SetFloat("EFFECT", dB);
        }
    }
}