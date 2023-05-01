using System;
using System.Collections;
using System.Collections.Generic;
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
        private Dictionary<string, float> engagementMap = new Dictionary<string, float>();

        private void Awake() {
            _sceneManager = FindObjectOfType<SceneManager>();
            EventManager.AddListener<MinigameFinishedEvent>(OnMinigameFinished);
        }

        private void Start() {
            
        }

        private void Update() {
            
        }

        private void OnMinigameFinished(MinigameFinishedEvent evt) {
            if (numberOfMinigameDone == 3) {
                
            } else {
                engagementMap[evt.Minigame] = evt.Engagement;
                numberOfMinigameDone++;
                isMainMenuLoaded = true;
                _sceneManager.LoadMainMenu();

                StartCoroutine(ShowRecap(evt.Recap));
            }
        }

        private IEnumerator ShowRecap(string recap) {
            yield return new WaitForSeconds(0.5f);
            
            _menuUIManager = FindObjectOfType<MainMenuUIManager>();
            _menuUIManager.ToggleNextButton();
            _menuUIManager.SetRecapText(recap);
        }

        public void LoadNextMinigame() {
            _sceneManager.LoadNextMinigameScene();
        }
        
        private void OnDestroy() {
            EventManager.RemoveListener<MinigameFinishedEvent>(OnMinigameFinished);
        }
    }
}