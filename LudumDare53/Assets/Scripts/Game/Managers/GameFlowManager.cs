using System;
using UI;
using UnityEngine;

namespace Game.Managers {
    public class GameFlowManager : MonoBehaviour {

        public GameObject transitionPanel;
        public GameObject infoButton;

        private SceneManager _sceneManager;
        private MainMenuUIManager _menuUIManager;
        private int numberOfMinigameDone = 0;
        private bool isMainMenuLoaded = false;
        private bool managerFound = false;

        private void Awake() {
            _sceneManager = FindObjectOfType<SceneManager>();
            EventManager.AddListener<MinigameFinishedEvent>(OnMinigameFinished);
        }

        private void Start() {
            
        }

        private void Update() {
            if (isMainMenuLoaded && !managerFound) {
                MainMenuUIManager mainMenuUIManager = FindObjectOfType<MainMenuUIManager>();
                if (mainMenuUIManager != null) {
                    managerFound = true;
                    
                    mainMenuUIManager.ToggleNextButton();
                }
            }
        }

        private void OnMinigameFinished(MinigameFinishedEvent evt) {
            if (numberOfMinigameDone == 3) {
                
            } else {
                numberOfMinigameDone++;
                isMainMenuLoaded = true;
                _sceneManager.LoadMainMenu();
            }
        }

        public void LoadNextMinigame() {
            _sceneManager.LoadNextMinigameScene();
        }
        
        private void OnDestroy() {
            EventManager.RemoveListener<MinigameFinishedEvent>(OnMinigameFinished);
        }
    }
}