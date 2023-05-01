using Game;
using Game.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManagerMinigame1 : MonoBehaviour
{
    public GameObject controls;
    public GameObject gameStats;
    public GameObject minigameOver;

    public GameObject timerGameObject;
    public GameObject spawnerGameObject;
    public GameObject engagementBarGameObject;
    public GameObject playerGameObject;

    public int gameDuration = 60;
    public float bonusPoints = 0.05f;
    public float malusPoints = -0.1f;

    private Spawner spawner;
    private TimerNew timer;
    private EngagementBar engagementBar;
    private CharacterControllerMiky characterController;

    private TextMeshProUGUI textMinigameOver;

    private bool gameStart = false;
    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        spawner = spawnerGameObject.GetComponent<Spawner>();
        timer = timerGameObject.GetComponent<TimerNew>();
        engagementBar = engagementBarGameObject.GetComponent<EngagementBar>();
        characterController = playerGameObject.GetComponent<CharacterControllerMiky>();

        foreach (TextMeshProUGUI t in minigameOver.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (t.name == "TextEngagement")
                textMinigameOver = t;
        }

        controls.SetActive(true);
        gameStats.SetActive(false);
        minigameOver.SetActive(false);

        playerGameObject.GetComponent<CharacterControllerMiky>().SetCollisionCallback((v) => {
            
            if (v > 0.0f)
            {
                engagementBar.AddPoints(bonusPoints);
            }
            else
            {
                engagementBar.AddPoints(malusPoints);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStart && (Input.GetKeyDown("a") || Input.GetKeyDown("d")))
        {
            gameStart = true;

            controls.SetActive(false);
            gameStats.SetActive(true);

            spawner.StartSpawner();

            timer.StartTimer(gameDuration);
            engagementBar.StartEngagementBar(1.0f);

            EventManager.AddListener<TimerTimeOutEvent>(OnTimeout);
        }

        if (gameOver)
        {
            if (spawner.GetSpawnedObj() == 0)
            {
                gameStats.SetActive(false);
                minigameOver.SetActive(true);

                engagementBar.StopEngagementBar();

                characterController.SetGameOver(true);
                textMinigameOver.text = "Engagement: " + (Mathf.Min(1.0f, engagementBar.GetCurrent()) * 100).ToString("F0") + "%";
            }
        }
    }

    private void OnTimeout(TimerTimeOutEvent e)
    {
        gameOver = true;
        spawner.StopSpawner();
        
        EventManager.RemoveListener<TimerTimeOutEvent>(OnTimeout);
    }
}
