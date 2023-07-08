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
using System.Linq;

public class GameManager : MonoBehaviour
{
    private bool paused = false;
    private bool gameOver = false;
    private int score = 0;
    private float secondsElapsed;
    private int minutesCounter;
    [SerializeField]
    private int remainingMeter = 60;
    [SerializeField]
    private int meterDrainRate = 1;
    private int MAX_METER = 100;
    private int currentPieces = 0;
    [SerializeField]
    private int maxPieces = 50;
    [SerializeField]
    private int tierThresholdValue = 10000;
    private int currentTier = 1;

    public InputManager _inputManager;
    public EventSystem eventSystem;
    public List<GameObject> spawnerGroups;

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
        UnityEngine.Debug.Log("Updating score view...");

        scoreText.text = "Score: " + score;
    }

    public void AddScore(int scoreValue)
    {
        UnityEngine.Debug.Log("Score was: " + score);

        score += scoreValue;
        
        UnityEngine.Debug.Log("Score is now: " + score);

        UpdateScoreText();

        var thresholdCrossed = score - tierThresholdValue > 0;
        if (thresholdCrossed && score < 50000)
        {
            UnityEngine.Debug.Log("Score was: " + score + " and therefore crossed a threshold");

            DetermineTier();
        }
    }

    public bool CheckForMaxPieces()
    {
        return currentPieces < maxPieces;
    }

    public void DetermineTier()
    {
        UnityEngine.Debug.Log("Determining new tier, it is currently: " + currentTier);

        switch (++currentTier)
        {
            case 2:
                UnityEngine.Debug.Log("Tier is now tier 2");

                tierThresholdValue = 10000;

                var tier2SpawnerGroups = spawnerGroups.Where(spawner => spawner.tag.Equals("Tier_2_Spawner")).ToList();
                for (int i = 0; i < tier2SpawnerGroups.Count; i++)
                {
                    tier2SpawnerGroups[i].SetActive(true);
                }

                var otherSpawnerGroups = spawnerGroups.Where(spawner => spawner.tag.Equals("Tier_1_Spawner")).ToList();
                for (int i = 0; i < otherSpawnerGroups.Count; i++)
                {
                    var spawnerChildren = otherSpawnerGroups[i].GetComponentsInChildren<FruitSpawner>();
                    for (int j = 0; j < spawnerChildren.Length; j++)
                    {
                        spawnerChildren[j].spawnerTimer = 2.5f;
                        spawnerChildren[j].maxSpawnCount = 6;
                        spawnerChildren[j].cooldownTimer = 5.0f;
                    }
                }

                break;
            case 3:
                UnityEngine.Debug.Log("Tier is now tier 3");

                tierThresholdValue = 30000;

                meterDrainRate = 3;

                var tier3SpawnerGroups = spawnerGroups.Where(spawner => spawner.tag.Equals("Tier_3_Spawner")).ToList();
                for (int i = 0; i < tier3SpawnerGroups.Count; i++)
                {
                    tier3SpawnerGroups[i].SetActive(true);
                }

                var tier1And2SpawnerGroups = spawnerGroups.Where(spawner => spawner.tag.Equals("Tier_2_Spawner")).ToList();
                tier1And2SpawnerGroups.AddRange(spawnerGroups.Where(spawner => spawner.tag.Equals("Tier_1_Spawner")).ToList());
                for (int i = 0; i < tier1And2SpawnerGroups.Count; i++)
                {
                    var spawnerChildren = tier1And2SpawnerGroups[i].GetComponentsInChildren<FruitSpawner>();
                    for (int j = 0; j < spawnerChildren.Length; j++)
                    {
                        spawnerChildren[j].spawnerTimer = 1.75f;
                        spawnerChildren[j].maxSpawnCount = 9;
                        spawnerChildren[j].cooldownTimer = 4.0f;
                    }
                }

                break;
            case 4:
                UnityEngine.Debug.Log("Tier is now tier 4");

                meterDrainRate = 5;

                var tier4SpawnerGroups = spawnerGroups.Where(spawner => spawner.tag.Equals("Tier_4_Spawner")).ToList();
                for (int i = 0; i < tier4SpawnerGroups.Count; i++)
                {
                    tier4SpawnerGroups[i].SetActive(true);
                }

                var tier1And2And3SpawnerGroups = spawnerGroups.Where(spawner => spawner.tag.Equals("Tier_1_Spawner")).ToList();
                tier1And2And3SpawnerGroups.AddRange(spawnerGroups.Where(spawner => spawner.tag.Equals("Tier_2_Spawner")).ToList());
                tier1And2And3SpawnerGroups.AddRange(spawnerGroups.Where(spawner => spawner.tag.Equals("Tier_3_Spawner")).ToList());
                for (int i = 0; i < tier1And2And3SpawnerGroups.Count; i++)
                {
                    var spawnerChildren = tier1And2And3SpawnerGroups[i].GetComponentsInChildren<FruitSpawner>();
                    for (int j = 0; j < spawnerChildren.Length; j++)
                    {
                        spawnerChildren[j].spawnerTimer = 1.75f;
                        spawnerChildren[j].maxSpawnCount = 9;
                        spawnerChildren[j].cooldownTimer = 4.0f;
                    }
                }

                break;
            default:
                break;
        }
    }

    private void SetInitialMeterView()
    {
        meterView.sprite = meterSprites[1];
    }

    private void DrainMeter()
    {
        remainingMeter -= meterDrainRate;

        UpdateMeterView();

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
            UnityEngine.Debug.Log("Meter is green now");

            meterView.sprite = meterSprites[0];
        }
        else if(remainingMeter > 50 && remainingMeter <= 75)
        {
            UnityEngine.Debug.Log("Meter is yellow now");

            meterView.sprite = meterSprites[1];
        }
        else if(remainingMeter > 25 && remainingMeter <= 50)
        {
            UnityEngine.Debug.Log("Meter is orange now");

            meterView.sprite = meterSprites[2];
        }
        else if(remainingMeter > 0 && remainingMeter <= 25)
        {
            UnityEngine.Debug.Log("Meter is red now");

            meterView.sprite = meterSprites[3];
        }
    }

    public void RecoverMeter(int recoverValue)
    {
        UnityEngine.Debug.Log("Recovery amount for meter is: " + recoverValue);

        var newMeterValue = remainingMeter + recoverValue;
        if (newMeterValue > MAX_METER)
        {
            UnityEngine.Debug.Log("Meter value has exceeded the max, setting to 100");

            remainingMeter = MAX_METER;
        }
        else
        {
            remainingMeter = newMeterValue;

            UnityEngine.Debug.Log("New meter value is now: " + remainingMeter);
        }

        UpdateMeterView();
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
