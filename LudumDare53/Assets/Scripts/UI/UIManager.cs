using System;
using Game;
using Game.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class UIManager : MonoBehaviour {
        
        public TextMeshProUGUI _textMeshProUGUI;

        private void Awake() {
            EventManager.AddListener<WallPassedEvent>(OnWallPassedEvent);
        }

        private void Start() {
            _textMeshProUGUI.text = "0";
        }

        private void OnWallPassedEvent(WallPassedEvent evt) {
            _textMeshProUGUI.text = "" + evt.Score;
        }

        private void OnDestroy() {
            EventManager.RemoveListener<WallPassedEvent>(OnWallPassedEvent);
        }
    }
}