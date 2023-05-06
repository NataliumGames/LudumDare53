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
    public GameObject multiplierCanvas;

    public ParticleSystem[] particleSystems;

    public float time = 20.0f;

    private AudioManager audioManager;
    private Timer timer;
    private CameraShake cameraShake;
    private PunchlineUI punchButton;

    private GaugeBar gaugeBar;
    private TextMeshProUGUI multiplierText;

    private int counter = 0;
    private int counterStep;

    private bool gameRunning = false;
    private bool gameOver = false;
    
    private float perfectScore;

    private float lastPressed = 0.0f; // timestamp representing the last time the user pressed the spacebar

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        timer = FindObjectOfType<Timer>();
        cameraShake = FindObjectOfType<CameraShake>();
        punchButton = FindObjectOfType<PunchlineUI>();

        gaugeBar = multiplierCanvas.GetComponentInChildren<GaugeBar>();
        multiplierText = multiplierCanvas.GetComponentInChildren<TextMeshProUGUI>();

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

        // show panels
        controlsGameObject.SetActive(true);
        gameStatsGameObject.SetActive(false);
        gameOverGameObject.SetActive(false);
    }

    void Update()
    {
        if (!gameRunning && !gameOver && Input.GetKeyDown("space"))
        {
            StartGame();
        }

        if (gameRunning && !gameOver)
        {
            if (Input.GetKeyDown("space"))
            {
                PunchLine();
            }

            if (Input.GetKeyUp("space"))
            {
                punchButton.Release();
            }
            
            //Debug.LogWarning("Cur Time:" + Time.realtimeSinceStartup);
            //Debug.LogWarning("Last pressed:" + lastPressed);

            if (Time.realtimeSinceStartup - lastPressed >= MAX_USER_DELAY) {
                //Debug.Log("Delay elapsed!");
            
                cameraShake.shakeAmount = 0.0f;
                // vfx
                foreach(ParticleSystem ps in particleSystems)
                {
                    var emission = ps.emission;
                    emission.rateOverTime = 0.0f;
                }
            }
        }

        // BRAINSTORMING

        // praticamente andrebbe qui
        // camerashake * counter ecc, se � startato

        // salvo timestamp ultima volta chiamato (camera shake)

        // se shake 250ms
        // se son passati 50ms dall'ultimo, richiamo lo shake

        // dovrei fare delle prove forse perch� ho paura che se duri poco poi vada tipo a scatti ripartendo sempre dalla posizione iniziale e
        // l'effetto sia bruttino

        // allora il camera shake funziona cos�:
        // c'� una durata totale e un'intensit�:
        // nella update lui diminuisce in base al deltatime in modo da fare un massimo di tempo pari alla durata complessiva

        // quindi a noi basta fare il toggle della durata (0/quanto vogliamo farlo durare - eventualmente anche il massimo, tipo 20, e quando l'utente
        // non clicca per un tot di tempo lo disattiviamo, o qualcosa del genere)

        // esatto, dobbiamo almeno tenere una variabile aggiuntiva tipo

        // yees, max 20 secondi

        // intensit� 0.0/1.0 toggle

        // aggiorniamo continuamente il timestamp dell'ultima volta che � stato premuto

        // controlliamo se:
        // il timestamp � all'interno della durata di shake
        // -> intensit� >0 => viene calcolata (come facciamo gi� in controller)
        
        // altrimenti intensit� =0

       
        // funzione da chiamare con argomento tempo (chiamante fa sia camera controller che vfx emitter)

        // ahah s�

        // s� adesso finisco una roba e ci provo (tutto sempre nella scena di test)
        
        // il trofeo ancora no, se avete un'idea di dove metterlo possiamo provare a farlo
        
        
        
        // TO-DO
    }

    private void StartGame()
    {
        controlsGameObject.SetActive(false);
        gameStatsGameObject.SetActive(true);

        gameRunning = true;

        timer.StartTimer();

        // start shake timer
        cameraShake.shakeDuration = time;
        cameraShake.shakeAmount = 0.0f;

        // start vfx timer
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }

        // start music
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
            //emission.rateOverTime = 220;
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
        // terminate vfx

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
}
