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
using System;

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
    public Image meterView;
    public List<Sprite> meterSprites;

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

    private void CheckPause()
    {
        var pauseButtonValue = _inputManager.pauseButton;
        if(pauseButtonValue == 1.0f)
        {
            Pause();

            _inputManager.pauseButton = 0.0f;
        }
    }

    private void DisplayGameOver()
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
        gameOver = false;

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

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    public void AddScore(int scoreValue)
    {
        score += scoreValue;

        UpdateScoreText();
    }

    public bool CheckForMaxPieces()
    {
        return currentPieces < maxPieces;
    }

    //TODO
    public void IncreaseMaxPieces()
    {
        // Need to determine score threshold arithmetic
    }

    private void SetInitialMeterView()
    {
        meterView.sprite = meterSprites[1];
    }

    private void DrainMeter()
    {
        var currentMeterValue = remainingMeter;
        var thresholdStatus = currentMeterValue % 25;

        --remainingMeter;

        var newThresholdStatus = remainingMeter % 25;
        if(thresholdStatus != 0 && newThresholdStatus == 0)
        {
            UpdateMeterView();
        }

        if(remainingMeter <= 0)
        {
            gameOver = true;

            DisplayGameOver();
        }
    }

    private void UpdateMeterView()
    {
        if (remainingMeter > 75 && remainingMeter <= 100)
        {
            meterView.sprite = meterSprites[0];
        }
        else if(remainingMeter > 50 && remainingMeter <= 75)
        {
            meterView.sprite = meterSprites[1];
        }
        else if(remainingMeter > 25 && remainingMeter <= 50)
        {
            meterView.sprite = meterSprites[2];
        }
        else if(remainingMeter > 0 && remainingMeter <= 25)
        {
            meterView.sprite = meterSprites[3];
        }
    }

    public void RecoverMeter(int recoverValue)
    {
        var currentMeterValue = remainingMeter;
        var thresholdStatus = currentMeterValue % 25;

        var newMeterValue = remainingMeter + recoverValue;
        if (newMeterValue > MAX_METER)
        {
            remainingMeter = MAX_METER;
        }
        else
        {
            remainingMeter = newMeterValue;
        }

        var newThresholdStatus = remainingMeter % 25;
        if (thresholdStatus != 0 && newThresholdStatus == 0)
        {
            UpdateMeterView();
        }
    }

    // A global instance for scripts to reference
    public static GameManager instance;

    /// <summary>
    /// Description:
    /// Standard Unity Function called when the script is loaded
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    private void Awake()
    {
        // Set up the instance of this
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetInitalScoreView();

        SetInitialMeterView();

        InvokeRepeating("DrainMeter", 0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateElapsedTime();
        
        CheckPause();
    }
}
