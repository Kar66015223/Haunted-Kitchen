using UnityEngine;

public class GhostSound : MonoBehaviour
{
    private AudioSource source;
    public AudioClip spawnSound;
    public AudioClip hitCrossSound;

    void Awake()
    {
        source = GetComponent<AudioSource>();

        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (source.isPlaying)
            source.Stop();

        source.clip = clip;

        if (!source.isPlaying)
        {
            source.Play();
        }
    }
}
