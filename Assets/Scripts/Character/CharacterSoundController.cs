using UnityEngine;

namespace Character
{
    public class CharacterSoundController : MonoBehaviour
    {
        public AudioClip jumpAudio;
        public AudioClip scoreHighlightAudio;
    
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }
    
        /// <summary>
        /// Play jump audio
        /// </summary>
        public void PlayJump()
        {
            audioSource.PlayOneShot(jumpAudio);
        }
    
        /// <summary>
        /// Play score highlight audio
        /// </summary>
        public void PlayScoreHighlight()
        {
            audioSource.PlayOneShot(scoreHighlightAudio);
        }
    }
}
