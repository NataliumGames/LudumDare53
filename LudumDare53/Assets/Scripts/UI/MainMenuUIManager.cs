using System;
using Game.Managers;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class MainMenuUIManager : MonoBehaviour {

        private GameFlowManager _gameFlowManager;
        private Button startButton;
        private Button nextButton;
        private Button quitButton;
        private Button infoButton;

        private GameObject startButtonGameobject;
        private GameObject nextButtonGameobject;

        private void Awake() {
            _gameFlowManager = FindObjectOfType<GameFlowManager>();
            
            startButtonGameobject = transform.GetChild(0).gameObject;
            nextButtonGameobject = transform.GetChild(1).gameObject;
            
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
        
        private void OnStartButtonPressed() {
            FindObjectOfType<AudioManager>().FadeOutMusic("MenuMusic", 1);
            FindObjectOfType<AudioManager>().PlayFX("Broue");
            
            _gameFlowManager.LoadNextMinigame();
        }

        private void OnNextButtonPressed() {
            Debug.Log("ciao");
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