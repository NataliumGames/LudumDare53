using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using Game.Managers;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;
using Timer = Game.Timer;

namespace Gameplay.Managers {
    public class StageObjectsManager : MonoBehaviour {

        public GameObject controls;
        public GameObject monitorPrefab;
        public GameObject micstandPrefab;
        public GameObject stoodPrefab;
        public List<Transform> monitorSpawnPoints;
        public List<Transform> micstandSpawnPoints;
        public List<Transform> stoodSpawnPoints;
        public bool gameIsRunning = false;
        public GaugeBarVertical engagementBar;
        private Engagement engagement;
        private CameraShake cameraShake;

        private List<GameObject> instantiatedGameobjects;

        private void Awake() {
            EventManager.AddListener<ObjectRepairedEvent>(OnObjectRepaired);
            EventManager.AddListener<TimerTimeOutEvent>(OnTimerTimeout);
            engagement = GetComponent<Engagement>();
            cameraShake = FindObjectOfType<CameraShake>();
            EventManager.AddListener<EngagementChangeEvent>(OnEngagementChange);
            EventManager.AddListener<HittedByEnemyEvent>(OnHittedByEnemyEvent);
        }
        
        private void Start() {
            instantiatedGameobjects = new List<GameObject>();

            // Monitor spawn
            for (int i = 0; i < 2; i++) {
                Transform t = monitorSpawnPoints[Random.Range(0, monitorSpawnPoints.Count - 1)];
                monitorSpawnPoints.Remove(t);
                GameObject monitor = Instantiate(monitorPrefab, t);
                instantiatedGameobjects.Add(monitor);
            }
            
            // Mic stand spawn
            Transform transform = micstandSpawnPoints[Random.Range(0, micstandSpawnPoints.Count - 1)];
            GameObject g = Instantiate(micstandPrefab, transform);
            instantiatedGameobjects.Add(g);
            
            // Stood spawn
            transform = stoodSpawnPoints[Random.Range(0, stoodSpawnPoints.Count - 1)];
            g = Instantiate(stoodPrefab, transform);
            instantiatedGameobjects.Add(g);
        }

        private void Update() {
            if (!gameIsRunning && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D))) {
                controls.SetActive(false);
                gameIsRunning = true;
                
                engagement.SetRunning(true);
                FindObjectOfType<Timer>().StartTimer();
                FindObjectOfType<EnemySpawner>().Spawn();
            }
        }
        
        private void OnEngagementChange(EngagementChangeEvent evt) {
            engagementBar.SetBarValue(evt.Value);
        }

        private void OnHittedByEnemyEvent(HittedByEnemyEvent evt) {
            cameraShake.Shake(0.1f);
            engagementBar.DecrementValueBy(0.2f);
        }

        private void EndGame() {
            MinigameFinishedEvent minigameFinishedEvent = Events.MinigameFinishedEvent;
            Engagement engagement = FindObjectOfType<Engagement>();
            minigameFinishedEvent.Engagement = engagement.engagement;
            minigameFinishedEvent.Minigame = "Fix Stage";
            minigameFinishedEvent.Time = FindObjectOfType<Timer>().timeRemaining;
            float eng = minigameFinishedEvent.Engagement * 100f;

            StringBuilder stringBuilder = new StringBuilder("<align=\"center\">" + minigameFinishedEvent.Minigame);
            stringBuilder.Append("\n\n\n");
            stringBuilder.Append("<align=\"left\"><color=\"red\">Engagement: <color=\"black\">" + eng.ToString("0.00") + "%");

            minigameFinishedEvent.Recap = stringBuilder.ToString();
            
            EventManager.Broadcast(minigameFinishedEvent);
        }

        private void OnTimerTimeout(TimerTimeOutEvent evt) {
            EndGame();
        }

        private void OnObjectRepaired(ObjectRepairedEvent evt) {
            GameObject g = instantiatedGameobjects.Find(obj => obj == evt.Object);
            
            g.tag = "Repaired";
            g.transform.GetChild(1).gameObject.SetActive(false);

            if (instantiatedGameobjects.All(obj => obj.tag.Equals("Repaired"))) {
                EndGame();
            }
        }

        private void OnDestroy() {
            EventManager.RemoveListener<ObjectRepairedEvent>(OnObjectRepaired);
            EventManager.RemoveListener<TimerTimeOutEvent>(OnTimerTimeout);
            EventManager.RemoveListener<EngagementChangeEvent>(OnEngagementChange);
            EventManager.RemoveListener<HittedByEnemyEvent>(OnHittedByEnemyEvent);
        }
    }
}