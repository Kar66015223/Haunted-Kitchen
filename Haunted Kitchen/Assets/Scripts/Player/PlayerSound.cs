using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioSource movementSource;
    [SerializeField] private AudioSource actionSource;

    public AudioClip walkSound;
    public AudioClip runSound;

    public AudioClip beginPossessedSound;
    public AudioClip mopSound;
    public AudioClip dropSound;

    public void PlaySound(AudioClip clip)
    {
        actionSource.PlayOneShot(clip);
    }

    public void PlayWalkSound(bool isRunning)
    {
        AudioClip targetClip = isRunning ? runSound : walkSound;

        if (movementSource.clip == targetClip && movementSource.isPlaying) return;

        movementSource.clip = targetClip;
        movementSource.loop = true;
        movementSource.volume = 0.3f;

        movementSource.Play();
    }

    public void PlayMopSound()
    {
        if (actionSource.clip == mopSound && actionSource.isPlaying) return;

        actionSource.clip = mopSound;
        actionSource.loop = true;
        actionSource.Play();
    }

    public void StopMovementSound() => movementSource.Stop();
    public void StopAction() => actionSource.Stop();

    public void StopAll()
    {
        movementSource.Stop();
        actionSource.Stop();
    }
}
