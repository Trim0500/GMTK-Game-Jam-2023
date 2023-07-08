using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private bool paused = false;
    private bool gameOver = false;
    private int score = 0;
    private float secondsElapsed;
    private int minutesCounter;
    private int remainingMeter = 60;
    private int MAX_METER = 100;
    private int currentPieces = 0;
    private int maxPieces = 10;

    public InputManager _inputManager;
    public EventSystem eventSystem;

    public TMP_Text currentTimeText;
    public TMP_Text scoreText;
    public GameObject pauseScreen;
    public GameObject pauseDefaultSelected;
    public GameObject gameOverScreen;
    public GameObject gameOverDefaultSelected;

    private void Pause()
    {
        if (!paused)
        {
            paused = true;

            Time.timeScale = 0f;

            pauseScreen.SetActive(true);

            eventSystem.SetSelectedGameObject(pauseDefaultSelected);
        }
        else
        {
            paused = false;

            Time.timeScale = 1.0f;

            pauseScreen.SetActive(false);
        }
    }

    public void CheckPause()
    {
        var pauseButtonValue = _inputManager.pauseButton;
        if(pauseButtonValue == 1.0f)
        {
            Pause();

            _inputManager.pauseButton = 0.0f;
        }
    }

    public void DisplayGameOver()
    {
        Time.timeScale = 0f;

        gameOverScreen.SetActive(true);

        eventSystem.SetSelectedGameObject(gameOverDefaultSelected);
    }

    public void ResumeButtonOnClick()
    {
        paused = false;

        Time.timeScale = 1.0f;

        pauseScreen.SetActive(false);
    }

    public void RestartButtonOnClick()
    {
        paused = false;

        Time.timeScale = 1.0f;

        SceneManager.LoadScene("Main_Game");
    }

    public void QuitButtonOnClick()
    {
        paused = false;

        Time.timeScale = 1.0f;

        SceneManager.LoadScene("Title_Screen");
    }

    private void UpdateElapsedTime()
    {
        var currentTime = secondsElapsed;
        var newTime = currentTime + Time.deltaTime;
        secondsElapsed = newTime;
        if(secondsElapsed >= 60.0f)
        {
            ++minutesCounter;

            secondsElapsed = 0.0f;
        }

        var secondsDisplay = "0";
        if(secondsElapsed < 10.0f)
        {
            secondsDisplay = "0" + (int)secondsElapsed;
        }
        else
        {
            var intSeconds = (int)secondsElapsed;
            secondsDisplay = intSeconds.ToString();
        }

        var minuteDisplay = "0";
        if(minutesCounter < 10)
        {
            minuteDisplay = "0" + minutesCounter.ToString();
        }
        else
        {
            minuteDisplay = minutesCounter.ToString();
        }

        currentTimeText.text = string.Format("Time: {0}:{1}", minuteDisplay, secondsDisplay);
    }

    private void SetInitalScoreView()
    {
        scoreText.text = "Score: 0";
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    public void AddScore(int scoreValue)
    {
        score += scoreValue;
    }

    public bool CheckForMaxPieces()
    {
        return currentPieces >= maxPieces;
    }

    //TODO
    public void IncreaseMaxPieces()
    {
        // Need to determine score threshold arithmetic
    }

    private void DrainMeter()
    {
        var modSeconds = secondsElapsed % 1;
        if(modSeconds == 0)
        {
            --remainingMeter;
        }

        if(remainingMeter <= 0)
        {
            gameOver = true;
        }
    }

    //TODO
    private void UpdateMeterView()
    {
        // Need to figure out what the meter sprite will be
    }

    public void RecoverMeter(int recoverValue)
    {
        var newMeterValue = remainingMeter + recoverValue;
        if(newMeterValue > MAX_METER)
        {
            remainingMeter = MAX_METER;
        }
        else
        {
            remainingMeter = newMeterValue;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetInitalScoreView();
    }

    // Update is called once per frame
    void Update()
    {
        DrainMeter();

        UpdateElapsedTime();
        
        CheckPause();
    }
}
