using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game.Managers {
    public class SceneManager : MonoBehaviour {

        private List<string> setupScenes = new List<string>();
        private string currentScene = "MainMenu";

        private void Start() {
            setupScenes.Add("SetupMinigameFallingObjects");
            setupScenes.Add("SetupMinigameWallsDodging");
            setupScenes.Add("SetupMinigameMonkeyFix");
        }

        public void LoadNextMinigameScene() {
            string minigame = setupScenes[Random.Range(0, setupScenes.Count)];
            setupScenes.Remove(minigame);
            currentScene = minigame;
            UnityEngine.SceneManagement.SceneManager.LoadScene(minigame, LoadSceneMode.Single);
        }

        public void LoadMainMenu() {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        public void LoadPunchline() {
            UnityEngine.SceneManagement.SceneManager.LoadScene("PunchlineMinigame", LoadSceneMode.Single);
        }
    }
}