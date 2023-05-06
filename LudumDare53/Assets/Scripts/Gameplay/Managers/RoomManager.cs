using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Game;
using Game.Managers;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Managers {
    
    public class RoomManager : MonoBehaviour {

        public float spawnDelay = 3f;
        public float increaseDifficultyTime = 10f;
        public float spawnDecreaseRate = 0.2f;
        public Transform spawnPos;
        public List<GameObject> wallPrefabs;

        public GameObject controls;
        public GameObject stats;

        private GaugeBarVertical engagementBar;
        private bool gameRunning = false;
        private bool gameOver = false;
        private List<GameObject> instantiatedWalls;

        private void Awake() {
            EventManager.AddListener<EngagementChangeEvent>(OnEngagementChange);
            EventManager.AddListener<TimerTimeOutEvent>(OnTimerTimeoutEvent);

            engagementBar = FindObjectOfType<GaugeBarVertical>();
        }

        private void Start() {
            instantiatedWalls = new List<GameObject>();

            controls.SetActive(true);
            stats.SetActive(false);
        }

        private void Update() {
            if (!gameRunning && !gameOver && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))) {
                controls.SetActive(false);
                stats.SetActive(true);
                gameRunning = true;

                StartCoroutine(SpawnWall());
                StartCoroutine(IncreaseDifficulty());
                FindObjectOfType<Engagement>().SetRunning(true);
                FindObjectOfType<Timer>().StartTimer();
            }
        }

        private void OnEngagementChange(EngagementChangeEvent evt) {
            engagementBar.SetBarValue(evt.Value);

            if (evt.Value <= 0.0f)
            {
                gameRunning = false;
                stats.SetActive(false);
                FindObjectOfType<Engagement>().SetRunning(false);
                FindObjectOfType<Timer>().StopTimer();

                StartCoroutine(ClearWalls(0.0f));

                GameOverEvent gameOverEvent = Events.GameOverEvent;
                EventManager.Broadcast(gameOverEvent);
            }
        }

        private void OnTimerTimeoutEvent(TimerTimeOutEvent evt) {
            gameRunning = false;
            gameOver = true;

            float engagement = FindObjectOfType<Engagement>().engagement;
            MinigameFinishedEvent minigameFinishedEvent = Events.MinigameFinishedEvent;
            minigameFinishedEvent.Engagement = engagement;
            minigameFinishedEvent.Minigame = "Dodge the Insults";
            float eng = minigameFinishedEvent.Engagement * 100f;
            if (eng >= 99.5f)
                eng = 100.0f;

            StringBuilder stringBuilder = new StringBuilder("<align=\"center\">" + minigameFinishedEvent.Minigame);
            stringBuilder.Append("\n\n\n");
            stringBuilder.Append("<align=\"left\"><color=\"red\">Engagement: <color=\"black\">" + eng.ToString("0.00") + "%");

            minigameFinishedEvent.Recap = stringBuilder.ToString();
            
            EventManager.Broadcast(minigameFinishedEvent);
        }

        private IEnumerator SpawnWall() {
            while (gameRunning) {
                Vector3 pos = new Vector3(0f, spawnPos.position.y, spawnPos.position.z);
                int randomIndex = Random.Range(0, wallPrefabs.Count);
                GameObject wall = Instantiate(wallPrefabs[randomIndex], pos, Quaternion.identity);
                wall.GetComponent<WallMover>().StartMovement();
                instantiatedWalls.Add(wall);
                StartCoroutine(DestroyGameobjectAfterDelay(wall, 3.5f));

                yield return new WaitForSeconds(spawnDelay);
            }
        }

        private IEnumerator IncreaseDifficulty() {
            bool running = true;

            while (running) {
                if (spawnDelay <= 0.5)
                    running = false;
                else
                    spawnDelay -= spawnDecreaseRate;

                yield return new WaitForSeconds(increaseDifficultyTime);
            }
        }

        private IEnumerator DestroyGameobjectAfterDelay(GameObject obj, float delay) {
            yield return new WaitForSeconds(delay);
            Destroy(obj);
            instantiatedWalls.Remove(obj);
        }

        private void OnDestroy() {
            EventManager.RemoveListener<EngagementChangeEvent>(OnEngagementChange);
            EventManager.RemoveListener<TimerTimeOutEvent>(OnTimerTimeoutEvent);
        }

        private IEnumerator ClearWalls(float delay)
        {
            yield return new WaitForSeconds(delay);

            foreach(GameObject go in GameObject.FindGameObjectsWithTag("Wall"))
            {
                Destroy(go);
            }
        }
    }
}