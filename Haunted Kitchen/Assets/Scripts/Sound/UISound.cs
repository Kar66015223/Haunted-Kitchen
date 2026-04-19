using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UISound : MonoBehaviour
{
    public static UISound instance;
    private AudioSource source;

    public AudioClip btnClickSound;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        
        if(source == null)
            source = GetComponent<AudioSource>();
    }

    public void PlayClickSound()
    {
        source.clip = btnClickSound;
        source.Play();
    }
}
