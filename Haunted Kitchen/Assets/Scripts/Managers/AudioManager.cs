using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource sourcePrefab;
    public AudioSource currentSource { get; private set; }

    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public void PlaySFX(AudioClip clip, Transform spawnTransform, float volume)
    {
        currentSource = Instantiate(
            sourcePrefab, spawnTransform.position, Quaternion.identity);

        currentSource.clip = clip;
        currentSource.volume = volume;

        currentSource.Play();

        float clipLength = currentSource.clip.length;
        Destroy(currentSource.gameObject, clipLength);
    }
}
