using System;
using Game.Managers;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class MainMenuUIManager : MonoBehaviour {

        private Button startButton;
        private Button quitButton;
        private Button infoButton;

        private void Awake() {
            startButton = transform.GetChild(0).GetComponent<Button>();
            quitButton = transform.GetChild(1).GetComponent<Button>();
            infoButton = transform.GetChild(2).GetComponent<Button>();
            
            startButton.onClick.AddListener(OnStartButtonPressed);
            quitButton.onClick.AddListener(OnQuitButtonPressed);
            infoButton.onClick.AddListener(OnInfoButtonPressed);
        }
        

        private void OnStartButtonPressed() {
            FindObjectOfType<AudioManager>().FadeOutMusic("MenuMusic", 1);
            FindObjectOfType<SceneManager>().LoadScene();
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