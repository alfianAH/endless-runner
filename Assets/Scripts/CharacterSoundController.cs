using UnityEngine;

public class CharacterSoundController : MonoBehaviour
{
    public AudioClip jumpAudio;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayJump()
    {
        audioSource.PlayOneShot(jumpAudio);
    }
}
