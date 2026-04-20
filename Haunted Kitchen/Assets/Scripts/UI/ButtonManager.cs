using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private List<Button> allButtons = new();

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        if (allButtons.Count > 0)
        {
            foreach (Button button in allButtons)
            {
                button.onClick.AddListener(SoundManager.instance.PlayClickSound);
            }
        }

        if(SoundMixerManager.instance != null)
        {
            masterSlider.value = SoundMixerManager.instance.MasterValue;
            musicSlider.value = SoundMixerManager.instance.MusicValue;
            sfxSlider.value = SoundMixerManager.instance.SfxValue;

            masterSlider.onValueChanged.AddListener(SoundMixerManager.instance.SetMasterVolume);
            musicSlider.onValueChanged.AddListener(SoundMixerManager.instance.SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(SoundMixerManager.instance.SetSFXVolume);
        }
    }
}
