using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        
        [SerializeField] private Text finalScore;
        [SerializeField] private Text finalAsteroids;
        [SerializeField] private Text finalTotalTime;
        [SerializeField] private Text newRecordText;

        [SerializeField] private Button restartLevel;

        private void Awake()
        {
            SetFinalScore();
        }

        private void OnEnable()
        {
            restartLevel.onClick.AddListener(RestartLevel);
        }

        private void SetFinalScore()
        {
            finalScore.text = $"FINAL SCORE : {levelManager.CurrentScore}";
            finalAsteroids.text = $"ASTEROIDS : {levelManager.EarnAsteroidsCount}";
            finalTotalTime.text = $"TOTAL TIME : {levelManager.TimeInGame : F2}";

            var lastBestScore = PlayerPrefs.GetInt("BestScore", 0);
            if (levelManager.CurrentScore <= lastBestScore) return;
            newRecordText.enabled = true;
        }

        private void RestartLevel()
        {
            var levelIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(levelIndex);
            Time.timeScale = 1;
        }
    }
}
