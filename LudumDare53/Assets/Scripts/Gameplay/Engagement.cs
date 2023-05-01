using System;
using System.Collections;
using Game;
using Game.Managers;
using UnityEngine;

namespace Gameplay {
    public class Engagement : MonoBehaviour {

        public float engagement = 1f;
        public float naturalDecrease = 0.05f;
        private bool gameIsRunning = false;

        private void Start() {
            
        }

        private void Update() {
            if (gameIsRunning) {
                engagement -= Time.deltaTime * 0.05f;
                BroadcastChange(engagement);
                
                if (engagement <= 0f) {
                    engagement = 0f;
                    gameIsRunning = false;
                    GameOverEvent gameOverEvent = Events.GameOverEvent;
                    EventManager.Broadcast(gameOverEvent);
                }

                if (engagement >= 1f) {
                    engagement = 1f;
                }
            }
        }

        public void SetRunning(bool value) {
            gameIsRunning = value;
        }

        public void IncrementValueBy(float value) {
            engagement += value;
            BroadcastChange(engagement);
        }

        public void DecrementValueBy(float value) {
            engagement -= value;
            BroadcastChange(engagement);
        }

        private void BroadcastChange(float value) {
            EngagementChangeEvent engagementChangeEvent = Events.EngagementChangeEvent;
            engagementChangeEvent.Value = value;
            EventManager.Broadcast(engagementChangeEvent);
        }
    }
}