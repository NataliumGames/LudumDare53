using System;
using Game;
using Game.Managers;
using UnityEngine;

namespace UI {
    public class FixingGameUIManager : MonoBehaviour {

        public GameObject gameoverPanel;

        private void Awake() {
            EventManager.AddListener<GameOverEvent>(OnGameOverEvent);
        }

        private void OnGameOverEvent(GameOverEvent gameOverEvent) {
            gameoverPanel.SetActive(true);
        }

        private void OnDestroy() {
            EventManager.RemoveListener<GameOverEvent>(OnGameOverEvent);
        }
    }
}