using Score;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class UiScoreController : MonoBehaviour
    {
        [Header("UI")] 
        public Text currentScore;
        public Text highScore;
    
        [Header("Score")] 
        private ScoreController scoreController;

        private void Start()
        {
            scoreController = ScoreController.Instance;
        }

        private void Update()
        {
            currentScore.text = scoreController.CurrentScore.ToString();
            highScore.text = ScoreData.highScore.ToString();
        }
    }
}
