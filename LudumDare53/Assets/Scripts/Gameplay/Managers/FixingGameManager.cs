using System;
using Game;
using Game.Managers;
using UI;
using UnityEngine;

namespace Gameplay.Managers {
    public class FixingGameManager : MonoBehaviour {

        public GaugeBar engagementBar;
        private Engagement engagement;
        private CameraShake cameraShake;

        private void Start() {
            engagement = GetComponent<Engagement>();
            cameraShake = FindObjectOfType<CameraShake>();
            EventManager.AddListener<EngagementChangeEvent>(OnEngagementChange);
            EventManager.AddListener<HittedByEnemyEvent>(OnHittedByEnemyEvent);
        }

        private void Update() {
            
        }

        private void OnEngagementChange(EngagementChangeEvent evt) {
            engagementBar.SetBarValue(evt.Value);
        }

        private void OnHittedByEnemyEvent(HittedByEnemyEvent evt) {
            cameraShake.Shake(1f);
        }

        private void OnDestroy() {
            EventManager.RemoveListener<EngagementChangeEvent>(OnEngagementChange);
            EventManager.RemoveListener<HittedByEnemyEvent>(OnHittedByEnemyEvent);
        }
    }
}