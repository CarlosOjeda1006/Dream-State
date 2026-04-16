using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioScripts : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioMixer mixer;

    [Header("Music Volume")]
    public string musicParameter = "MusicVolume"; // nombre expuesto en el mixer
    public Slider musicSlider;

    [Header("SFX Volume")]
    public string sfxParameter = "SFXVolume"; // nombre expuesto en el mixer
    public Slider sfxSlider;

    private void Start()
    {
        // --- MÚSICA ---
        if (musicSlider != null)
        {
            float savedMusic = PlayerPrefs.GetFloat(musicParameter, 0.75f);
            musicSlider.value = savedMusic;
            SetMusicVolume(savedMusic);
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        // --- EFECTOS DE SONIDO ---
        if (sfxSlider != null)
        {
            float savedSFX = PlayerPrefs.GetFloat(sfxParameter, 0.75f);
            sfxSlider.value = savedSFX;
            SetSFXVolume(savedSFX);
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    public void SetMusicVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1)) * 20;
        mixer.SetFloat(musicParameter, dB);
        PlayerPrefs.SetFloat(musicParameter, volume);
    }

    public void SetSFXVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1)) * 20;
        mixer.SetFloat(sfxParameter, dB);
        PlayerPrefs.SetFloat(sfxParameter, volume);
    }
}
