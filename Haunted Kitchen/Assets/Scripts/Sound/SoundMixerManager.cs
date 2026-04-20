using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerManager : MonoBehaviour
{
    public static SoundMixerManager instance;

    [SerializeField] private AudioMixer mixer;

    [SerializeField] private Slider masterSlider;
    public float MasterValue { get; private set; } = 1f;

    [SerializeField] private Slider musicSlider;
    public float MusicValue { get; private set; } = 1f;

    [SerializeField] private Slider sfxSlider;
    public float SfxValue { get; private set; } = 1f;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if(masterSlider != null)
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
        if(musicSlider != null)
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        if(sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float value)
    {
        MasterValue = value;
        mixer.SetFloat("MasterVolume", MathF.Log10(value) * 20f);
    }

    public void SetMusicVolume(float value)
    {
        MusicValue = value;
        mixer.SetFloat("MusicVolume", MathF.Log10(value) * 20f);
    }
    
    public void SetSFXVolume(float value)
    {
        SfxValue = value;
        mixer.SetFloat("SFXVolume", MathF.Log10(value) * 20f);
    }
}
