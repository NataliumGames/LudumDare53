using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Game.Managers {
    public class GameFlowManager : MonoBehaviour {

        public GameObject infoButton;

        private SceneManager _sceneManager;
        private MainMenuUIManager _menuUIManager;
        public int numberOfMinigameDone = 0;
        private bool isMainMenuLoaded = false;
        private bool managerFound = false;
        public Dictionary<string, float> engagementMap = new Dictionary<string, float>();

        private void Awake() {
            _sceneManager = FindObjectOfType<SceneManager>();
            EventManager.AddListener<MinigameFinishedEvent>(OnMinigameFinished);
        }

        private void Start() {
            infoButton.SetActive(true);
        }

        private void Update() {
            
        }

        private void OnMinigameFinished(MinigameFinishedEvent evt)
        {
            if (numberOfMinigameDone != 2)
            {
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

        public void LoadPunchline() {
            _sceneManager.LoadPunchline();
        }
        
        private void OnDestroy() {
            EventManager.RemoveListener<MinigameFinishedEvent>(OnMinigameFinished);
        }
    }
}