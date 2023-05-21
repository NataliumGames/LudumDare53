using System;
using System.Collections;
using Game;
using Game.Managers;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI {
    public class GameOverUI : MonoBehaviour {

        public GameObject gameoverPanel;
        private Button startButton;
        private Button quitButton;
        private TextMeshProUGUI startButtonText;

        private int startTimeout;

        private void Awake() {
            EventManager.AddListener<GameOverEvent>(OnGameOverEvent);
            startButton = gameoverPanel.transform.GetChild(1).GetComponent<Button>();
            quitButton = gameoverPanel.transform.GetChild(2).GetComponent<Button>();
            startButtonText = startButton.GetComponentInChildren<TextMeshProUGUI>();

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

            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                startTimeout = 3;

                startButtonText.text = "" + startTimeout;
                startButton.interactable = false;
                startButton.gameObject.GetComponent<EventTrigger>().enabled = false;

                StartCoroutine(StartTimer());
            }
        }

        IEnumerator StartTimer()
        {
            while (true)
            {
                if (startTimeout == 0)
                    break;

                startButtonText.text = "" + startTimeout;
                startTimeout--;

                yield return new WaitForSeconds(1);
            }

            startButtonText.text = "Menu";
            startButton.interactable = true;
            startButton.gameObject.GetComponent<EventTrigger>().enabled = true;
        }

        private void OnDestroy() {
            EventManager.RemoveListener<GameOverEvent>(OnGameOverEvent);
        }
    }
}