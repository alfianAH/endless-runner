using Character;
using UnityEngine;

namespace Score
{
    public class ScoreController : MonoBehaviour
    {
        [Header("Score Highlight")] 
        public int scoreHighlightRange;
        private CharacterSoundController sound;

        private int currentScore;
        private int lastScoreHighlight;
    
        public int CurrentScore => currentScore;

        private void Start()
        {
            sound = CharacterSoundController.Instance;
            
            currentScore = 0;
            lastScoreHighlight = 0;
        }
    
        /// <summary>
        /// Increase current score by increment
        /// </summary>
        /// <param name="increment">Value to add to the score </param>
        public void IncreaseCurrentScore(int increment)
        {
            currentScore += increment;
        
            // Play sound every score highlight range
            if (currentScore - lastScoreHighlight > scoreHighlightRange)
            {
                sound.PlayScoreHighlight();
                lastScoreHighlight += scoreHighlightRange;
            }
        }
    
        /// <summary>
        /// Set high score if finish
        /// </summary>
        public void FinishScoring()
        {
            // Set high score
            if (currentScore > ScoreData.highScore)
            {
                ScoreData.highScore = currentScore;
            }
        }
    }
}
