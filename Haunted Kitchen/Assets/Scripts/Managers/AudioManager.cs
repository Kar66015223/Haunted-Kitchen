using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource sourcePrefab;

    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public void PlaySFX(AudioClip clip, Transform spawnTransform, float volume)
    {
        AudioSource source = Instantiate(
            sourcePrefab, spawnTransform.position, Quaternion.identity);

        source.clip = clip;
        source.volume = volume;

        source.Play();

        float clipLength = source.clip.length;
        Destroy(source.gameObject, clipLength);
    }
}
