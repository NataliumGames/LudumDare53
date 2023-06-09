using Game;
using Game.Managers;
using Managers;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using TMPro;
using UI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManagerPunchline : MonoBehaviour
{
    /*
     * The world record for CPS (Clicks Per Second) by a human is 14.1, 
     * while the average is 6.51 CPS.
     * 
     * 10/11 might be a great goal to achieve the 100%.
     */ 
    private const float PERFECT_CPS = 11.0f;
    private const float MAX_SHAKE_AMOUNT = 0.2f;
    private const int MAX_PARTICLES = 220;
    private const float MAX_USER_DELAY = 0.25f; // delay after which the shake/vfx stops if the user doesn't press the spacebar

    public GameObject controlsGameObject;
    public GameObject gameStatsGameObject;
    public GameObject gameOverGameObject;
    public TextMeshProUGUI gameOverDescription;
    public Button mainMenuButton;
    public Button quitButton;

    public GameObject multiplierCanvas;
    public ParticleSystem[] particleSystems;

    public float time = 20.0f;

    private AudioManager audioManager;
    private Timer timer;
    private CameraShake cameraShake;
    private PunchlineUI punchButton;

    private GaugeBar gaugeBar;
    private TextMeshProUGUI multiplierText;
    private TextMeshProUGUI mainMenuButtonText;

    private int counter = 0;
    private int counterStep;

    private bool gameRunning = false;
    private bool gameOver = false;
    
    private float perfectScore;

    private float lastPressed = 0.0f; // timestamp representing the last time the user pressed the spacebar
    private int mainMenuTimeout;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        timer = FindObjectOfType<Timer>();
        cameraShake = FindObjectOfType<CameraShake>();
        punchButton = FindObjectOfType<PunchlineUI>();

        gaugeBar = multiplierCanvas.GetComponentInChildren<GaugeBar>();
        multiplierText = multiplierCanvas.GetComponentInChildren<TextMeshProUGUI>();
        mainMenuButtonText = mainMenuButton.GetComponentInChildren<TextMeshProUGUI>();

        perfectScore = PERFECT_CPS * time;
        counterStep = (int) perfectScore / 5;

        timer.timeRemaining = time;
        cameraShake.shakeDuration = 0.0f;
        cameraShake.shakeAmount = 0.0f;

        for(int i = 0; i < particleSystems.Length; i++)
        {
            var loop = particleSystems[i].main.loop;
            var emission = particleSystems[i].emission;
            var duration = particleSystems[i].main.duration;

            loop = false;
            emission.rateOverTime = 0.0f;
            duration = time;
        }
        for (int i = 0; i < controlsGameObject.transform.childCount; i++)
        {
            GameObject go = controlsGameObject.transform.GetChild(i).gameObject;

            if (go.name.ToLower().Contains("desktop"))
                go.SetActive(SystemInfo.deviceType == DeviceType.Desktop);
            if (go.name.ToLower().Contains("mobile"))
                go.SetActive(SystemInfo.deviceType == DeviceType.Handheld);
        }

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            punchButton.SetButtonDownCallback(PunchLine);
            punchButton.SetButtonUpCallback(punchButton.Release);
        }

        mainMenuButton.onClick.AddListener(MenuButton);
        quitButton.onClick.AddListener(QuitButton);

        // Show panels
        controlsGameObject.SetActive(true);
        gameStatsGameObject.SetActive(false);
        gameOverGameObject.SetActive(false);
    }

    void Update()
    {
        if (!gameRunning && !gameOver 
            && (Input.GetKeyDown("space") || (SystemInfo.deviceType == DeviceType.Handheld && Input.GetMouseButtonDown(0))))
        {
            StartGame();
        }

        if (gameRunning && !gameOver)
        {
            if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                if (Input.GetKeyDown("space"))
                {
                    PunchLine();
                }

                if (Input.GetKeyUp("space"))
                {
                    punchButton.Release();
                }
            }

            if (Time.realtimeSinceStartup - lastPressed >= MAX_USER_DELAY)
            {
                cameraShake.shakeAmount = 0.0f;
                foreach(ParticleSystem ps in particleSystems)
                {
                    var emission = ps.emission;
                    emission.rateOverTime = 0.0f;
                }
            }
        }
    }

    private void StartGame()
    {
        controlsGameObject.SetActive(false);
        gameStatsGameObject.SetActive(true);

        gameRunning = true;

        timer.StartTimer();

        // Start shake timer
        cameraShake.shakeDuration = time;
        cameraShake.shakeAmount = 0.0f;

        // Start vfx timer
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }

        // Start music
        FindObjectOfType<AudioManager>().StopAll();
        FindObjectOfType<AudioManager>().PlayMusic("Punchline");

        EventManager.AddListener<TimerTimeOutEvent>(OnTimerTimeoutEvent);
    }

    private void PunchLine()
    {
        // Press button
        punchButton.Press();
        counter++;

        // Update GaugeBar & Multiplier
        gaugeBar.SetBarValue(counter / perfectScore);
        multiplierText.SetText("x" + updateMultiplier());

        // Shake Camera
        cameraShake.shakeAmount = (counter / perfectScore) * MAX_SHAKE_AMOUNT;
        lastPressed = Time.realtimeSinceStartup;

        // Show VFX
        foreach (ParticleSystem ps in particleSystems)
        {
            var emission = ps.emission;
            emission.rateOverTime = (counter / perfectScore) * MAX_PARTICLES;
        }

        // Play sound
        audioManager.PlayPunch();
    }

    private void OnTimerTimeoutEvent(TimerTimeOutEvent evt)
    {
        GameFlowManager gameFlowManager;
        float average = 0.0f, multiplier = 0.0f, finalScore = 0.0f;

        gameRunning = false;
        gameOver = true;
        punchButton.Release();
        punchButton.gameObject.SetActive(false);

        cameraShake.shakeAmount = 0.0f;
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Stop(true);
        }
        

        gameFlowManager = FindObjectOfType<GameFlowManager>();
        if (gameFlowManager != null)
        {
            average = gameFlowManager.engagementMap.Values.Average() * 100f;
            multiplier = updateMultiplier();
            finalScore = average * multiplier;
        }

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("Average Engagement: " + average.ToString("0.00") + "\n");
        stringBuilder.Append("Punchline Multiplier: X" + multiplier + "\n\n");
        stringBuilder.Append("Final Score: " + finalScore.ToString("0.00") + "\n");

        gameOverDescription.text = stringBuilder.ToString();

        gameStatsGameObject.SetActive(false);
        gameOverGameObject.SetActive(true);

        EventManager.RemoveListener<TimerTimeOutEvent>(OnTimerTimeoutEvent);

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            mainMenuTimeout = 3;

            mainMenuButton.interactable = false;
            mainMenuButton.gameObject.GetComponent<EventTrigger>().enabled = false;

            StartCoroutine(MainMenuTimer());
        }
    }

    IEnumerator MainMenuTimer()
    {
        while (true)
        {
            if (mainMenuTimeout == 0)
                break;

            mainMenuButtonText.text = "" + mainMenuTimeout;
            mainMenuTimeout--;

            yield return new WaitForSeconds(1);
        }

        mainMenuButtonText.text = "Menu";
        mainMenuButton.interactable = true;
        mainMenuButton.gameObject.GetComponent<EventTrigger>().enabled = true;
    }

    private float updateMultiplier()
    {
        float multiplier = 0.0f;
        if (counter >= counterStep)
        {
            multiplier = 1.0f;
        }
        if (counter >= counterStep * 2)
        {
            multiplier = 2.0f;
        }
        if (counter >= counterStep * 3)
        {
            multiplier = 4.0f;
        }
        if (counter >= counterStep * 4)
        {
            multiplier = 8.0f;
        }
        if (counter >= counterStep * 4.9f)
        {
            multiplier = 10.0f;
        }

        return multiplier;
    }
    
    private void MenuButton()
    {
        audioManager.PlayFX("Snap");
        audioManager.PlayMusic("MenuMusic");

        FindObjectOfType<SceneManager>().LoadMainMenu();
    }

    private void QuitButton()
    {
        Application.Quit();
    }
}
