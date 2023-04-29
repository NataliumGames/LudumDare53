using System;
using Game;
using Game.Managers;
using TMPro;
using UnityEngine;

namespace UI {
    public class UIManager : MonoBehaviour {
        
        public TextMeshProUGUI _textMeshProUGUI;

        private void Awake() {
            EventManager.AddListener<WallPassedEvent>(OnWallPassedEvent);
        }

        private void Start() {
            _textMeshProUGUI.text = "Score: 0";
        }

        private void OnWallPassedEvent(WallPassedEvent evt) {
            _textMeshProUGUI.text = "Score: " + evt.Score;
        }
        
        private void OnDestroy() {
            EventManager.RemoveListener<WallPassedEvent>(OnWallPassedEvent);
        }
    }
}