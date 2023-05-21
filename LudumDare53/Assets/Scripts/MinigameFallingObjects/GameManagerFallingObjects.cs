using Game;
using Game.Managers;
using Managers;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UI;
using UnityEngine;

public class GameManagerFallingObjects : MonoBehaviour
{
    public GameObject controlsGameObject;
    public GameObject gameStatsGameObject;
    public GameObject levelCompletedGameObject;
    public GameObject gameOverGameObject;

    public GameObject playerGameObject;

    public float gameDuration = 40;
    public float decreaseInTime = 0.04f;
    public float bonusEngagement = 0.05f;
    public float malusEngagement = -0.1f;
    public int bonusScore = 1;
    public int malusScore = -1;

    private AudioManager audioManager;
    private Spawner spawner;
    private Timer timer;
    private GaugeBarVertical gaugeBar;
    private CharacterControllerFallingObjects characterController;
    private CameraShake cameraShake;
    private FloatingJoystick floatingJoystick;

    private TextMeshProUGUI textCurrentScore;
    private TextMeshProUGUI textLevelCompletedScoreValue;
    private TextMeshProUGUI textLevelCompletedEngagementValue;

    private bool gameRunning = false;
    private bool levelCompleted = false;
    private bool gameOver = false;

    private int score = 0;

    void Start()
    {
        characterController = playerGameObject.GetComponent<CharacterControllerFallingObjects>();

        spawner = FindObjectOfType<Spawner>();
        audioManager = FindObjectOfType<AudioManager>();
        gaugeBar = FindObjectOfType<GaugeBarVertical>();
        timer = FindObjectOfType<Timer>();
        cameraShake = FindObjectOfType<CameraShake>();
        floatingJoystick = FindObjectOfType<FloatingJoystick>();

        timer.timeRemaining = gameDuration;
        gaugeBar.value = 1.0f;

        foreach (TextMeshProUGUI t in gameStatsGameObject.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (t.name == "TextCurrentScore")
                textCurrentScore = t;
        }
        foreach (TextMeshProUGUI t in levelCompletedGameObject.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (t.name == "TextScoreValue")
                textLevelCompletedScoreValue = t;
            if (t.name == "TextEngagementValue")
                textLevelCompletedEngagementValue = t;
        }
        for (int i = 0; i < controlsGameObject.transform.childCount; i++)
        {
            GameObject go = controlsGameObject.transform.GetChild(i).gameObject;

            if (go.name.ToLower().Contains("desktop"))
                go.SetActive(SystemInfo.deviceType == DeviceType.Desktop);
            if (go.name.ToLower().Contains("mobile"))
                go.SetActive(SystemInfo.deviceType == DeviceType.Handheld);
        }

        controlsGameObject.SetActive(true);
        gameStatsGameObject.SetActive(false);
        levelCompletedGameObject.SetActive(false);
        gameOverGameObject.SetActive(false);

        playerGameObject.GetComponent<CharacterControllerFallingObjects>().SetCollisionCallback((v) => {
            
            if (gameRunning)
            {
                if (v > 0.0f)
                {
                    audioManager.PlayBurp();
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
            }
        });
    }

    void Update()
    {
        if (timer.timeRemaining <= spawner.spawnRate * 3)
        {
            spawner.StopMalusSpawn();
        }

        // Press one movement key to start the minigame
        if (!gameRunning && !levelCompleted && !gameOver
            && (Input.GetKeyDown("a") || Input.GetKeyDown("d")
                || floatingJoystick.Horizontal != 0.0f))
        {
            gameRunning = true;

            controlsGameObject.SetActive(false);
            gameStatsGameObject.SetActive(true);

            spawner.StartSpawner();

            timer.StartTimer();
            gaugeBar.SetBarValue(1.0f);
            textCurrentScore.text = "" + score;

            EventManager.AddListener<TimerTimeOutEvent>(OnTimeout);
        }

        // Game loop
        if (gameRunning)
        {
            gaugeBar.DecrementValueBy(decreaseInTime * Time.deltaTime);
        }

        if (gaugeBar.value <= 0.0 && !gameOver)
        {
            gameRunning = false;
            gameOver = true;

            floatingJoystick.gameObject.SetActive(false);

            gameStatsGameObject.SetActive(false);

            spawner.StopSpawner();

            characterController.SetGameRunning(true);

            GameOverEvent gameOverEvent = Events.GameOverEvent;
            EventManager.Broadcast(gameOverEvent);

            timer.StopTimer();
            EventManager.RemoveListener<TimerTimeOutEvent>(OnTimeout);
        }

        // Level is completed successfully
        if (levelCompleted && !gameOver)
        {
            if (spawner.GetSpawnedObj() == 0)
            {
                gameRunning = false;
                levelCompleted = true;

                //gameStatsGameObject.SetActive(false);
                //levelCompletedGameObject.SetActive(true);

                characterController.SetGameRunning(true);
                textLevelCompletedScoreValue.text = "" + score;
                textLevelCompletedEngagementValue.text = (Mathf.Min(1.0f, gaugeBar.value) * 100).ToString("F0") + "%";

                // Finished event
                MinigameFinishedEvent minigameFinishedEvent = Events.MinigameFinishedEvent;
                minigameFinishedEvent.Minigame = "DroppingObjects";
                minigameFinishedEvent.Engagement = gaugeBar.value;
                float eng = minigameFinishedEvent.Engagement * 100f;
                if (eng >= 99.5f)
                    eng = 100.0f;
                
                StringBuilder stringBuilder = new StringBuilder("<align=\"center\">" + minigameFinishedEvent.Minigame);
                stringBuilder.Append("\n\n\n");
                stringBuilder.Append("<align=\"left\"><color=\"red\">Engagement: <color=\"black\">" + eng.ToString("0.00") + "%");

                minigameFinishedEvent.Recap = stringBuilder.ToString();
                
                EventManager.Broadcast(minigameFinishedEvent);
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
