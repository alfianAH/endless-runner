using UnityEngine;

public class CharacterSoundController : MonoBehaviour
{
    public AudioClip jumpAudio;
    public AudioClip scoreHighlightAudio;
    
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayJump()
    {
        audioSource.PlayOneShot(jumpAudio);
    }

    public void PlayScoreHighlight()
    {
        audioSource.PlayOneShot(scoreHighlightAudio);
    }
}
