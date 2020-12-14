using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UImanager : MonoBehaviour,IPointerClickHandler
{
    public Action OnLevelReset = delegate { };
    
    [Header("Prestart")]
    [SerializeField] CanvasGroup prestartMenu;

    [Header("Die")]
    [SerializeField] CanvasGroup dieMenu;
    [SerializeField] float waitTime = 2f;
    [SerializeField] float restartTime = 1f;
    [SerializeField] Text finalScore;
    [SerializeField] Text finalAsteroids;
    [SerializeField] Text finalTotalTime;
    [SerializeField] Text upgradeScore;

    [Header("SettingsPanel")]
    [SerializeField] CanvasGroup settingsPanel;
   
    [SerializeField] private AudioClip clickSound;

    Spaceship spaceship;

    
    public void ActivatePrestartMenu()
    {
        OnLevelReset();
        Time.timeScale = 0;
        prestartMenu.gameObject.SetActive(true);
    }
    public void StartGame()
    {
        if (!Input.anyKey) return;
        prestartMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
        spaceship = FindObjectOfType<Spaceship>();
        
    }
    public void RestartLevel()
    {
        OnLevelReset();
        Time.timeScale = 1;
        upgradeScore.gameObject.SetActive(false);
        dieMenu.gameObject.SetActive(false);
    }
    public void ActivateSettingPanel()
    {
        Time.timeScale = 0;
        settingsPanel.gameObject.SetActive(true);
        
    }
    public void CloseSettingPanel()
    {
        Time.timeScale = 1;
        settingsPanel.gameObject.SetActive(false);
    }
    
    public void ActivateLoseGamePanel()
    {
        StartCoroutine(EnableLosePanel(waitTime));
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        //AudioManager.Instance.PlayEffect(clickSound);
    }

    public void OnClick()
    {
        AudioManager.Instance.PlayEffect(clickSound);
    }
    
    

    private IEnumerator EnableLosePanel(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0;
        dieMenu.gameObject.SetActive(true);
        //finalScore.text = "FINAL SCORE : " + LevelManager.Instance.Score;
        //finalAsteroids.text = "ASTEROIDS : " + LevelManager.Instance.AsteroidsCount;
        //finalTotalTime.text = "TOTAL TIME : " + LevelManager.Instance.TimeInGame;
       // if (LevelManager.Instance.Score > PlayerPrefs.GetInt("BestScore", 0))
        {
            upgradeScore.gameObject.SetActive(true);
        }
    }
    
}
