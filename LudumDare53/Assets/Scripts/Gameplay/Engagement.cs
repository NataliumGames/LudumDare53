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

        private void BeginDecrease() {
            StartCoroutine(DecreaseOverTime());
        }

        private void BroadcastChange(float value) {
            EngagementChangeEvent engagementChangeEvent = Events.EngagementChangeEvent;
            engagementChangeEvent.Value = value;
            EventManager.Broadcast(engagementChangeEvent);
        }

        // TODO: gameover
        private IEnumerator DecreaseOverTime() {
            while (true) {
                engagement -= naturalDecrease;
                BroadcastChange(engagement);

                yield return new WaitForSeconds(1);
            }
        }
    }
}