using System;
using Game.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game {
    public class Timer : MonoBehaviour {

        public TextMeshProUGUI TextMeshProUGUI;
        public float timeRemaining = 60;
        private bool timerIsRunning = false;

        private void Start() {
            timerIsRunning = true;
        }

        private void Update() {
            if (timerIsRunning) {
                if (timeRemaining > 0) {
                    timeRemaining -= Time.deltaTime;
                    DisplayTime(timeRemaining);
                }
                else {
                    Debug.Log("Time has run out!");
                    
                    BroadcastTimeout();
                    timeRemaining = 0;
                    timerIsRunning = false;
                }
            }
        }

        private void DisplayTime(float timeToDisplay) {
            timeToDisplay += 1;
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            TextMeshProUGUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        private void BroadcastTimeout() {
            TimerTimeOutEvent timerTimeOutEvent = Events.TimerTimeOutEvent;
            EventManager.Broadcast(timerTimeOutEvent);
        }
    }
}