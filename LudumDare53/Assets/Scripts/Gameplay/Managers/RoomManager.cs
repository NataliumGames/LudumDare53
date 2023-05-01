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

        private GaugeBar engagementBar;
        private List<GameObject> instantiatedWalls;

        private void Awake() {
            EventManager.AddListener<EngagementChangeEvent>(OnEngagementChange);
            EventManager.AddListener<TimerTimeOutEvent>(OnTimerTimeoutEvent);

            engagementBar = FindObjectOfType<GaugeBar>();
        }

        private void Start() {
            instantiatedWalls = new List<GameObject>();

            StartCoroutine(SpawnWall());
            StartCoroutine(IncreaseDifficulty());
        }

        private void OnEngagementChange(EngagementChangeEvent evt) {
            engagementBar.SetBarValue(evt.Value);
        }

        private void OnTimerTimeoutEvent(TimerTimeOutEvent evt) {
            float engagement = FindObjectOfType<Engagement>().engagement;
            MinigameFinishedEvent minigameFinishedEvent = Events.MinigameFinishedEvent;
            minigameFinishedEvent.Engagement = engagement;
            minigameFinishedEvent.Minigame = "Dodge the Insults";
            
            StringBuilder stringBuilder = new StringBuilder("<align=\"center\">" + minigameFinishedEvent.Minigame);
            stringBuilder.Append("\n\n\n");
            stringBuilder.Append("<align=\"left\"><color=\"red\">Engagement: <color=\"black\">" + minigameFinishedEvent.Engagement * 100 + "%");

            minigameFinishedEvent.Recap = stringBuilder.ToString();
            
            EventManager.Broadcast(minigameFinishedEvent);
        }

        private IEnumerator SpawnWall() {
            while (true) {
                Vector3 pos = new Vector3(Random.Range(-2f, 1f), spawnPos.position.y, spawnPos.position.z);
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
    }
}