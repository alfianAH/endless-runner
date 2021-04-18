using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private int currentScore;

    public int CurrentScore
    {
        get => currentScore;
        set => currentScore = value;
    }

    private void Start()
    {
        currentScore = 0;
    }

    public void IncreaseCurrentScore(int increment)
    {
        currentScore += increment;
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
