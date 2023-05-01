using Game;
using Game.Managers;
using Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

public class GameManagerFallingObjects : MonoBehaviour
{
    public GameObject controlsGameObject;
    public GameObject gameStatsGameObject;
    public GameObject levelCompletedGameObject;
    public GameObject gameOverGameObject;

    public GameObject timerGameObject;
    public GameObject spawnerGameObject;
    public GameObject playerGameObject;

    public int gameDuration = 60;
    public float decreaseInTime = 0.04f;
    public float bonusEngagement = 0.05f;
    public float malusEngagement = -0.1f;
    public int bonusScore = 1;
    public int malusScore = -1;

    private AudioManager audioManager;
    private Spawner spawner;
    private Timer timer;
    private GaugeBar gaugeBar;
    private CharacterControllerMiky characterController;
    private CameraShake cameraShake;

    private TextMeshProUGUI textCurrentScore;
    private TextMeshProUGUI textCurrentTime;
    private TextMeshProUGUI textLevelCompletedScoreValue;
    private TextMeshProUGUI textLevelCompletedEngagementValue;

    private bool gameRunning = false;
    private bool levelCompleted = false;
    private bool gameOver = false;

    private int score = 0;

    private enum GameState {
        GAME_BEFORE_START,
        GAME_RUNNING,
        GAME_OVER,
        GAME_COMPLETED
    }

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        spawner = spawnerGameObject.GetComponent<Spawner>();
        timer = timerGameObject.GetComponent<Timer>();
        gaugeBar = FindObjectOfType<GaugeBar>();
        characterController = playerGameObject.GetComponent<CharacterControllerMiky>();

        cameraShake = FindObjectOfType<CameraShake>();

        foreach (TextMeshProUGUI t in gameStatsGameObject.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (t.name == "TextCurrentScore")
                textCurrentScore = t;
            if (t.name == "TextCurrentTime")
                textCurrentTime = t;
        }
        foreach (TextMeshProUGUI t in levelCompletedGameObject.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (t.name == "TextScoreValue")
                textLevelCompletedScoreValue = t;
            if (t.name == "TextEngagementValue")
                textLevelCompletedEngagementValue = t;
        }

        controlsGameObject.SetActive(true);
        gameStatsGameObject.SetActive(false);
        levelCompletedGameObject.SetActive(false);
        gameOverGameObject.SetActive(false);

        playerGameObject.GetComponent<CharacterControllerMiky>().SetCollisionCallback((v) => {
            
            if (v > 0.0f)
            {
                //audioManager.PlayFX("");
                gaugeBar.IncrementValueBy(bonusEngagement);
                score += bonusScore;
            }
            else
            {
                audioManager.PlayDamage();
                cameraShake.Shake(0.1f);
                gaugeBar.IncrementValueBy(malusEngagement);
                score += malusScore;
            }
            textCurrentScore.text = "" + score;
        });
    }

    void Update()
    {
        // Press one movement key to start the minigame
        if (!gameRunning && (Input.GetKeyDown("a") || Input.GetKeyDown("d")))
        {
            gameRunning = true;

            controlsGameObject.SetActive(false);
            gameStatsGameObject.SetActive(true);

            spawner.StartSpawner();

            timerGameObject.SetActive(true);
            //timer.StartTimer(gameDuration);
            gaugeBar.SetBarValue(1.0f);
            textCurrentScore.text = "" + score;

            EventManager.AddListener<TimerTimeOutEvent>(OnTimeout);
        }

        // Game loop
        if (gameRunning)
        {
            gaugeBar.DecrementValueBy(decreaseInTime * Time.deltaTime);
        }

        if (gaugeBar.value <= 0.0)
        {
            gameRunning = false;
            gameOver = true;

            gameStatsGameObject.SetActive(false);
            gameOverGameObject.SetActive(true);

            characterController.SetGameRunning(true);

            EventManager.RemoveListener<TimerTimeOutEvent>(OnTimeout);
        }

        // Level is completed successfully
        if (levelCompleted)
        {
            if (spawner.GetSpawnedObj() == 0)
            {
                gameRunning = false;
                gameStatsGameObject.SetActive(false);
                levelCompletedGameObject.SetActive(true);

                characterController.SetGameRunning(true);
                textLevelCompletedScoreValue.text = "" + score;
                textLevelCompletedEngagementValue.text = (Mathf.Min(1.0f, gaugeBar.value) * 100).ToString("F0") + "%";

                // Finished event
                MinigameFinishedEvent minigameFinishedEvent = Events.MinigameFinishedEvent;
                minigameFinishedEvent.Minigame = "DroppingObjects";
                minigameFinishedEvent.Engagement = gaugeBar.value;
                //minigameFinishedEvent.Score
                EventManager.Broadcast(minigameFinishedEvent);
            }
        }

        // Game over (loss)
        if (gameOver)
        {
            if (spawner.GetSpawnedObj() == 0)
            {
                gameStatsGameObject.SetActive(false);
                gameOverGameObject.SetActive(true);


                EventManager.RemoveListener<TimerTimeOutEvent>(OnTimeout);

                GameOverEvent gameOverEvent = Events.GameOverEvent;
                EventManager.Broadcast(gameOverEvent);
            }
        }
    }

    private void OnTimeout(TimerTimeOutEvent e)
    {
        levelCompleted = true;
        spawner.StopSpawner();
        
        EventManager.RemoveListener<TimerTimeOutEvent>(OnTimeout);
    }
}
