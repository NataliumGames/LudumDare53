using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game.Managers {
    public class SceneManager : MonoBehaviour {

        /*
        public static SceneManager Instance;

        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        */

        private List<string> setupScenes = new List<string>();
        private string currentScene = "MainMenu";

        private void Start() {
            setupScenes.Add("SetupMinigameMuri");
            setupScenes.Add("SetupMinigameMonkeyFix");
            setupScenes.Add("SetupMinigameMiky");
        }

        public void LoadNextMinigameScene() {
            string minigame = setupScenes[Random.Range(0, setupScenes.Count - 1)];
            setupScenes.Remove(minigame);
            currentScene = minigame;
            UnityEngine.SceneManagement.SceneManager.LoadScene(minigame, LoadSceneMode.Single);
        }

        public void LoadMainMenu() {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        public void LoadPunchline() {
            
        }
    }
}