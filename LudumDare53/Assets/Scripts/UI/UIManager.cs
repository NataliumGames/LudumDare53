using System;
using Game;
using Game.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class UIManager : MonoBehaviour {
        
        public TextMeshProUGUI _textMeshProUGUI;
        public GameObject gameoverPanel;

        private void Awake() {
            EventManager.AddListener<WallPassedEvent>(OnWallPassedEvent);
            EventManager.AddListener<GameOverEvent>(OnGameOverEvent);
        }

        private void Start() {
            _textMeshProUGUI.text = "0";
        }

        private void OnWallPassedEvent(WallPassedEvent evt) {
            _textMeshProUGUI.text = "" + evt.Score;
        }

        private void OnGameOverEvent(GameOverEvent evt) {
            gameoverPanel.SetActive(true);
        }
        
        private void OnDestroy() {
            EventManager.RemoveListener<WallPassedEvent>(OnWallPassedEvent);
            EventManager.RemoveListener<GameOverEvent>(OnGameOverEvent);
        }
    }
}