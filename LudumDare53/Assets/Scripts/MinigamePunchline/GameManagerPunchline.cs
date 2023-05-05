using Game;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerPunchline : MonoBehaviour
{
    /*
     * The world record for CPS (Clicks Per Second) by a human is 14.1, 
     * while the average is 6.51 CPS.
     * 
     * 10/11 might be a great goal to achieve the 100%.
     */ 
    private const float PERFECT_CPS = 11.0f;

    public GameObject controlsGameObject;
    public GameObject gameStatsGameObject;

    public float time = 20.0f;

    private AudioManager audioManager;
    private Timer timer;
    private CameraShake cameraShake;
    private PunchlineUI punchButton;

    private int counter = 0;
    // spacebar component

    private bool gameRunning = false;
    
    private float perfectScore;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        timer = FindObjectOfType<Timer>();
        cameraShake = FindObjectOfType<CameraShake>();
        punchButton = FindObjectOfType<PunchlineUI>();

        perfectScore = PERFECT_CPS * time;
        timer.timeRemaining = time;

        // show
        controlsGameObject.SetActive(true);
        gameStatsGameObject.SetActive(false);
    }


    void Update()
    {
        if (!gameRunning && Input.GetKeyDown("space"))
        {
            StartGame();
        }

        if (gameRunning)
        {
            if (Input.GetKeyDown("space"))
            {
                punchButton.Press();
            }

            if (Input.GetKeyUp("space"))
            {
                punchButton.Release();
            }
        }
    }

    private void StartGame()
    {
        controlsGameObject.SetActive(false);
        gameStatsGameObject.SetActive(true);

        gameRunning = true;

        timer.StartTimer();

        cameraShake.shakeDuration = 20.0f;
        cameraShake.shakeAmount = 0.0f;

        // start music
    }

    private void PunchLine()
    {
        // Press button
        punchButton.Press();
        counter++;

        // Show VFX
        // Shake Camera
        // Play sound
    }

    private void OnTimerTimeoutEvent(TimerTimeOutEvent evt)
    {
        punchButton.Release();
    }
}
