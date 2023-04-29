using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        private void Update() {
            Debug.Log("Scene Manager");
        }

        private void Start() {
        }

        public void LoadScene() {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SetupMinigame", LoadSceneMode.Single);
        }

        public void LoadMainMenu() {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Prova", LoadSceneMode.Single);
        }
    }
}