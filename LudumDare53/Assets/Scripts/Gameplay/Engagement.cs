using System;
using System.Collections;
using Game;
using Game.Managers;
using UnityEngine;

namespace Gameplay {
    public class Engagement : MonoBehaviour {

        public float engagement = 1f;
        public float naturalDecrease = 0.05f;

        private void Start() {
            Invoke("BeginDecrease", 2f);
        }

        private void Update() {
            if (engagement <= 0f) {
                engagement = 0f;
                GameOverEvent gameOverEvent = Events.GameOverEvent;
                EventManager.Broadcast(gameOverEvent);
            }
        }

        public void IncrementValueBy(float value) {
            engagement += value;
            BroadcastChange(engagement);
        }

        public void DecrementValueBy(float value) {
            engagement += value;
            BroadcastChange(engagement);
        }

        private void BeginDecrease() {
            StartCoroutine(DecreaseOverTime());
        }

        private void BroadcastChange(float value) {
            EngagementChangeEvent engagementChangeEvent = Events.EngagementChangeEvent;
            engagementChangeEvent.Value = value;
            EventManager.Broadcast(engagementChangeEvent);
        }

        private IEnumerator DecreaseOverTime() {
            while (true) {
                engagement -= naturalDecrease;
                BroadcastChange(engagement);

                yield return new WaitForSeconds(1);
            }
        }
    }
}