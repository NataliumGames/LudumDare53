using System;
using Game.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class MainMenuUIManager : MonoBehaviour {

        private Button startButton;
        private Button quitButton;

        private void Awake() {
            startButton = transform.GetChild(0).GetComponent<Button>();
            quitButton = transform.GetChild(1).GetComponent<Button>();
            
            startButton.onClick.AddListener(OnStartButtonPressed);
            quitButton.onClick.AddListener(OnQuitButtonPressed);
        }

        private void OnStartButtonPressed() {
            FindObjectOfType<SceneManager>().LoadScene();
        }

        private void OnQuitButtonPressed() {
            Debug.Log("Closing game...");
            Application.Quit();
        }
    }
}