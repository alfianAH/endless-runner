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
        public ScoreController scoreController;
    
        private void Update()
        {
            currentScore.text = scoreController.CurrentScore.ToString();
            highScore.text = ScoreData.highScore.ToString();
        }
    }
}
