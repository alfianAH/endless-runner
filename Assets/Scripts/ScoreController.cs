using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [Header("Score Highlight")] 
    public int scoreHighlightRange;
    public CharacterSoundController sound;

    private int currentScore;
    private int lastScoreHighlight;
    
    public int CurrentScore => currentScore;

    private void Start()
    {
        currentScore = 0;
        lastScoreHighlight = 0;
    }

    public void IncreaseCurrentScore(int increment)
    {
        currentScore += increment;

        if (currentScore - lastScoreHighlight > scoreHighlightRange)
        {
            sound.PlayScoreHighlight();
            lastScoreHighlight += scoreHighlightRange;
        }
    }

    public void FinishScoring()
    {
        // Set high score
        if (currentScore > ScoreData.HighScore)
        {
            ScoreData.HighScore = currentScore;
        }
    }
}
