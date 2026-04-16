using Unity.VisualScripting;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioSource source;

    public AudioClip walkSound;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    public void PlayWalkSound(bool isRunning)
    {
        if (source.clip != walkSound)
        {
            source.clip = walkSound;
            source.loop = true;
        }

        source.pitch = isRunning ? 1.5f : 1.0f;

        if (!source.isPlaying)
            source.Play();
    }

    public void StopSound()
    {
        if (source.isPlaying)
            source.Stop();
    }
}
