using UnityEngine;
using UnityEngine.UI;

public class VolumeUI : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider effectSlider;

    [SerializeField] private Toggle bgmMuteToggle;
    [SerializeField] private Toggle effectMuteToggle;

    private float lastBGMSliderValue = 1f;
    private float lastEffectSliderValue = 1f;

    void Start()
    {
        float bgm = PlayerPrefs.GetFloat("BGM_VOL", 1f);
        float effect = PlayerPrefs.GetFloat("EFFECT_VOL", 1f);

        bgmSlider.value = bgm;
        effectSlider.value = effect;

        SLIDER_ModifyBGMVolume();
        SLIDER_ModifyEffectVolume();

        bool bgmMute = PlayerPrefs.GetInt("BGM_MUTE", 0) == 1;
        bool effectMute = PlayerPrefs.GetInt("EFFECT_MUTE", 0) == 1;

        bgmMuteToggle.isOn = bgmMute;
        effectMuteToggle.isOn = effectMute;

        ToggleBGMMute();
        ToggleEffectMute();
    }

    public void SLIDER_ModifyBGMVolume()
    {
        float value = bgmSlider.value;
        float dB = Mathf.Lerp(-80f, 0f, value);
        SoundManager.Instance.SetVolume(SoundType.BGM, dB);

        PlayerPrefs.SetFloat("BGM_VOL", value);
        if (!bgmMuteToggle.isOn)
        {
            lastBGMSliderValue = value;
        }
    }
    public void SLIDER_ModifyEffectVolume()
    {
        float value = effectSlider.value;
        float dB = Mathf.Lerp(-80f, 0f, value);
        SoundManager.Instance.SetVolume(SoundType.EFFECT, dB);

        PlayerPrefs.SetFloat("EFFECT_VOL", value);
        if (!effectMuteToggle.isOn)
        {
            lastEffectSliderValue = value;
        }
    }

    public void ToggleBGMMute()
    {
        bool mute = bgmMuteToggle.isOn;
        if (mute)
        {
            lastBGMSliderValue = bgmSlider.value;
            bgmSlider.interactable = false;
            SoundManager.Instance.SetBGMMute(true);
        }
        else
        {
            bgmSlider.value = lastBGMSliderValue;
            bgmSlider.interactable = true;
            SLIDER_ModifyBGMVolume();
        }

        PlayerPrefs.SetInt("BGM_MUTE", mute ? 1 : 0);
    }

    public void ToggleEffectMute()
    {
        bool mute = effectMuteToggle.isOn;

        if (mute)
        {
            lastEffectSliderValue = effectSlider.value;
            effectSlider.interactable = false;
            SoundManager.Instance.SetEffectMute(true);
        }
        else
        {
            effectSlider.value = lastEffectSliderValue;
            effectSlider.interactable = true;
            SLIDER_ModifyEffectVolume();
        }

        PlayerPrefs.SetInt("EFFECT_MUTE", mute ? 1 : 0);
    }
}