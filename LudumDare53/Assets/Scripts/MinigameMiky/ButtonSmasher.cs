using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UI;
using Managers;
using Game;
using Game.Managers;
using UnityEngine.UI;


public class ButtonSmasher : MonoBehaviour {

    public GameObject recapPanel;
    public TextMeshProUGUI description;
    public Button menuButton;
    public Button quitButton;

    public int counter = 0;
    public GameObject gaugeBarObject;

    public int maxCounterValue;

    public GameObject multiplierTextObject;
    public GameObject controls;

    private AudioManager audioManager;

    private int counterStep;
    private bool gameIsRunning = false;

    private GaugeBar gaugeBar;
    private TextMeshProUGUI multiplierText;

    private void Awake() {
        EventManager.AddListener<TimerTimeOutEvent>(OnTimerTimeoutEvent);
    }

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();

        menuButton.onClick.AddListener(MenuButton);
        quitButton.onClick.AddListener(QuitButton);
        gaugeBar = gaugeBarObject.GetComponent<GaugeBar>();
        multiplierText = multiplierTextObject.GetComponent<TextMeshProUGUI>();

        counterStep = (int)maxCounterValue/5;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameIsRunning && Input.GetKeyDown(KeyCode.Space)) {
            gameIsRunning = true;
            controls.SetActive(false);
            FindObjectOfType<AudioManager>().StopAll();
            FindObjectOfType<AudioManager>().PlayMusic("Punchline");
            FindAnyObjectByType<Timer>().StartTimer();
        }
        
        if(gameIsRunning && Input.GetKeyDown("space"))
        {  
            counter++;
            gaugeBar.IncrementValueBy(1.0f/maxCounterValue);

            int multiplier = updateMultiplier();
            multiplierText.SetText("x"+multiplier);

            FindObjectOfType<AudioManager>().PlayPunch();
        }
    }
    
    private void OnTimerTimeoutEvent(TimerTimeOutEvent evt) {
        // calculate points
        GameFlowManager gameFlowManager = FindObjectOfType<GameFlowManager>();
        if(gameFlowManager == null)
            return;

        float media = gameFlowManager.engagementMap.Values.Average() * 100f;
        int multiplier = updateMultiplier();
        float finalScore = media * multiplier;

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("Average Engagement: " + media.ToString("0.00") + "\n");
        stringBuilder.Append("Punchline Multiplier: X" + multiplier + "\n\n");
        stringBuilder.Append("Final Score: " + finalScore.ToString("0.00") + "\n");

        description.text = stringBuilder.ToString();

        recapPanel.SetActive(true);
    }

    private int updateMultiplier() {
        int multiplier = 0;
        if (counter >= counterStep) {
            multiplier = 1;
        }
        if (counter >= counterStep*2) {
            multiplier = 2;
        }
        if (counter >= counterStep*3) {
            multiplier = 4;
        }
        if (counter >= counterStep*4) {
            multiplier = 8;
        }
        if (counter >= counterStep*5) {
            multiplier = 10;
        }

        return multiplier;
    }

    private void MenuButton() {
        audioManager.PlayMusic("MenuMusic");

        FindObjectOfType<SceneManager>().LoadMainMenu();
    }

    private void QuitButton() {
        Application.Quit();
    }

    private void OnDestroy() {
        EventManager.RemoveListener<TimerTimeOutEvent>(OnTimerTimeoutEvent);
    }
}
