using System;
using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour,IPointAdd
{
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnBestScoreChanged;
    public static event Action<int> OnAsteroidsCountChanged;
    public static event Action<float> OnTimeInGameChanged;
    
    [SerializeField] private int currentScore ;
    [SerializeField] private int currentBestScore ;
    [SerializeField] private int earnAsteroidsCount ;
    [SerializeField] private float timeInGame ;

    private float scoreTimer;

    public int CurrentScore => currentScore;
    public int CurrentBestScore => currentBestScore;
    public int EarnAsteroidsCount => earnAsteroidsCount;
    public float TimeInGame => timeInGame;
    
    public bool DoublePoints { get; set; }
    public void AddPointsToScore(int value)
    {
        currentScore += value;
        OnScoreChanged?.Invoke(currentScore);
        CheckNewRecord();
    }
    public void AddAsteroidsCount(int value)
    {
        earnAsteroidsCount += value;
        OnAsteroidsCountChanged?.Invoke(earnAsteroidsCount);
    }

    private void Start()
    {
        DoublePoints = false;
        ResetScores();
    }
    private void Update()
    {
        SetPointsToScore();
        SetTimeInGame();
    }
    private void SetPointsToScore()
    {
        scoreTimer += 1 * Time.deltaTime;
        if (!(scoreTimer >= 1)) return;
        //add points to score depending on the speed of movement
        if (DoublePoints == false)
        {
            currentScore += 1;
            scoreTimer = 0;
        }
        else if (DoublePoints)
        {
            currentScore += 2;
            scoreTimer = 0;
        }
        OnScoreChanged?.Invoke(currentScore);
        CheckNewRecord();
    }

    private void CheckNewRecord()
    {
        if (currentScore <= PlayerPrefs.GetInt("BestScore", 0)) return;
        PlayerPrefs.SetInt("BestScore", currentBestScore);
        OnBestScoreChanged?.Invoke(currentScore);
    }
    private void SetTimeInGame()
    {
        //adds 1 point every second
        timeInGame += 1 * Time.deltaTime;
        OnTimeInGameChanged?.Invoke(timeInGame);
    }
    private void ResetScores()
    {
        StartCoroutine(ResetScoreWithDelay());
    }
    
    private IEnumerator ResetScoreWithDelay() 
    {
        yield return new WaitForSeconds(0.3f);
        currentScore = 0;
        currentBestScore = 0;
        earnAsteroidsCount = 0;
        timeInGame = 0;
    }
}
