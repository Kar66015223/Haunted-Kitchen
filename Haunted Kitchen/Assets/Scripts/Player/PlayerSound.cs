using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioSource source;

    public AudioClip walkSound;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip, bool isLoop)
    {
        source.clip = clip;
        source.loop = isLoop;

        source.Play();
    }
}
