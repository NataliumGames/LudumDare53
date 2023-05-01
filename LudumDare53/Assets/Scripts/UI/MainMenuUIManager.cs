using System;
using Game.Managers;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class MainMenuUIManager : MonoBehaviour {

        private GameFlowManager _gameFlowManager;
        private Button startButton;
        private Button nextButton;
        private Button quitButton;
        private Button infoButton;
        private TextMeshProUGUI recapText;

        private GameObject startButtonGameobject;
        private GameObject nextButtonGameobject;
        private GameObject recapPanel;

        private void Awake() {
            _gameFlowManager = FindObjectOfType<GameFlowManager>();
            
            startButtonGameobject = transform.GetChild(0).gameObject;
            nextButtonGameobject = transform.GetChild(1).gameObject;
            recapPanel = transform.GetChild(5).gameObject;
            
            recapText = recapPanel.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
            startButton = startButtonGameobject.GetComponent<Button>();
            nextButton = nextButtonGameobject.GetComponent<Button>();
            quitButton = transform.GetChild(2).GetComponent<Button>();
            infoButton = transform.GetChild(3).GetComponent<Button>();
            
            startButton.onClick.AddListener(OnStartButtonPressed);
            nextButton.onClick.AddListener(OnNextButtonPressed);
            quitButton.onClick.AddListener(OnQuitButtonPressed);
            infoButton.onClick.AddListener(OnInfoButtonPressed);
        }

        public void ToggleNextButton() {
            startButtonGameobject.SetActive(false);
            nextButtonGameobject.SetActive(true);
        }

        public void SetRecapText(string text) {
            recapPanel.SetActive(true);
            recapText.text = text;
        }
        
        private void OnStartButtonPressed() {
            FindObjectOfType<AudioManager>().TransitionMusic("MainMusic");
            FindObjectOfType<AudioManager>().PlayFX("Broue");
            
            _gameFlowManager.LoadNextMinigame();
        }

        private void OnNextButtonPressed() {
            recapPanel.SetActive(false);
            _gameFlowManager.LoadNextMinigame();
        }
        
        private void OnInfoButtonPressed() {
            // TODO: show overlay with tutorial and info
        }

        private void OnQuitButtonPressed() {
            Debug.Log("Closing game...");
            Application.Quit();
        }
    }
}