using System;
using Game;
using Game.Managers;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class GameOverUI : MonoBehaviour {

        public GameObject gameoverPanel;
        private Button startButton;
        private Button quitButton;

        private void Awake() {
            EventManager.AddListener<GameOverEvent>(OnGameOverEvent);
            startButton = gameoverPanel.transform.GetChild(1).GetComponent<Button>();
            quitButton = gameoverPanel.transform.GetChild(2).GetComponent<Button>();
            
            startButton.onClick.AddListener(StartButtonPressed);
            quitButton.onClick.AddListener(QuitButtonPressed);
        }

        private void StartButtonPressed()
        {
            
            SceneManager sceneManager = FindObjectOfType<SceneManager>();
            if (sceneManager != null)
            {
                AudioManager audioManager = FindAnyObjectByType<AudioManager>();
                audioManager.StopAll();
                audioManager.PlayMusic("MenuMusic");
                sceneManager.LoadMainMenu();
            }
        }

        private void QuitButtonPressed() {
            Application.Quit();
        }

        private void OnGameOverEvent(GameOverEvent gameOverEvent) {
            gameoverPanel.SetActive(true);
        }

        private void OnDestroy() {
            EventManager.RemoveListener<GameOverEvent>(OnGameOverEvent);
        }
    }
}