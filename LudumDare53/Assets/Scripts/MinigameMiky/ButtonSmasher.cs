using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UI;
using Managers;
using Game;
using Game.Managers;


public class ButtonSmasher : MonoBehaviour
{

    public int counter = 0;
    public GameObject gaugeBarObject;

    public int maxCounterValue;

    public GameObject multiplierTextObject;

    private int counterStep;

    private GaugeBar gaugeBar;
    private TextMeshProUGUI multiplierText;

    private void Awake() {
        EventManager.AddListener<TimerTimeOutEvent>(OnTimerTimeoutEvent);
    }

    // Start is called before the first frame update
    void Start()
    {
        gaugeBar = gaugeBarObject.GetComponent<GaugeBar>();
        multiplierText = multiplierTextObject.GetComponent<TextMeshProUGUI>();

        counterStep = (int)maxCounterValue/5;

        FindAnyObjectByType<Timer>().StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space"))
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

        float media = gameFlowManager.engagementMap.Values.Average();

        // show final recap
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

    private void OnDestroy() {
        EventManager.RemoveListener<TimerTimeOutEvent>(OnTimerTimeoutEvent);
    }
}
