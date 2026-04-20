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
            if (masterSlider != null)
                masterSlider.value = SoundMixerManager.instance.MasterValue;
            if(musicSlider != null)  
                musicSlider.value = SoundMixerManager.instance.MusicValue;
            if(sfxSlider != null) 
                sfxSlider.value = SoundMixerManager.instance.SfxValue;

            if (masterSlider != null)
                masterSlider.onValueChanged.AddListener(SoundMixerManager.instance.SetMasterVolume);
            if(musicSlider != null)  
                musicSlider.onValueChanged.AddListener(SoundMixerManager.instance.SetMusicVolume);
            if(sfxSlider != null) 
                sfxSlider.onValueChanged.AddListener(SoundMixerManager.instance.SetSFXVolume);
        }
    }
}
