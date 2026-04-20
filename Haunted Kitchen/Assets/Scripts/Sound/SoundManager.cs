using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource SfxSource;
    [SerializeField] private AudioSource musicSource;

    public AudioClip btnClickSound;

    public AudioClip normalMusic;
    public AudioClip ghostPhaseMusic;

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
        PlayMusic(normalMusic);
    }

    public void PlayClickSound()
    {
        SfxSource.clip = btnClickSound;
        SfxSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip && musicSource.isPlaying)
            return;
        
        musicSource.clip = clip;
        musicSource.Play();
    }
}
