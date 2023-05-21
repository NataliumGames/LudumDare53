using System;
using System.Collections;
using Game.Managers;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI {
    public class MainMenuUIManager : MonoBehaviour {

        private GameFlowManager _gameFlowManager;

        private GameObject startButtonGameobject;
        private GameObject nextButtonGameobject;
        private GameObject recapPanelGameobject;

        private Button startButton;
        private Button nextButton;
        private Button quitButton;
        private Button infoButton;
        private TextMeshProUGUI recapText;

        private void Awake() {
            _gameFlowManager = FindObjectOfType<GameFlowManager>();
            
            startButtonGameobject = transform.GetChild(0).gameObject;
            nextButtonGameobject = transform.GetChild(1).gameObject;
            recapPanelGameobject = transform.GetChild(4).gameObject;
            
            recapText = recapPanelGameobject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
            startButton = startButtonGameobject.GetComponent<Button>();
            nextButton = nextButtonGameobject.GetComponent<Button>();
            quitButton = transform.GetChild(2).GetComponent<Button>();
            infoButton = transform.GetChild(3).GetComponent<Button>();

            startButton.onClick.AddListener(OnStartButtonPressed);
            nextButton.onClick.AddListener(OnNextButtonPressed);
            quitButton.onClick.AddListener(OnQuitButtonPressed);
            infoButton.onClick.AddListener(OnInfoButtonPressed);
        }

        public void ShowNextButton() {
            startButtonGameobject.SetActive(false);
            nextButtonGameobject.SetActive(true);
        }

        public void SetRecapText(string text) {
            recapPanelGameobject.SetActive(true);
            recapText.text = text;
        }
        
        private void OnStartButtonPressed() {
            FindObjectOfType<AudioManager>().TransitionMusic("MainMusic");
            FindObjectOfType<AudioManager>().PlayFX("Broue");

            infoButton.gameObject.SetActive(false);
            _gameFlowManager.LoadNextMinigame();
        }

        private void OnNextButtonPressed() {
            FindObjectOfType<AudioManager>().PlayFX("Broue");
            recapPanelGameobject.SetActive(false);

            if (_gameFlowManager.numberOfMinigameDone == 2) {
                infoButton.gameObject.SetActive(true);
                _gameFlowManager.LoadPunchline();
                _gameFlowManager.numberOfMinigameDone = 0;
            }
            else
            {
                infoButton.gameObject.SetActive(false);
                _gameFlowManager.LoadNextMinigame();
            }
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