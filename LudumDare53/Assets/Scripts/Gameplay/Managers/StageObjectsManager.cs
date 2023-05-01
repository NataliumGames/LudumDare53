using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Managers;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Managers {
    public class StageObjectsManager : MonoBehaviour {

        public GameObject monitorPrefab;
        public GameObject micstandPrefab;
        public GameObject stoodPrefab;
        public GameObject bottlePrefab;
        public List<Transform> monitorSpawnPoints;
        public List<Transform> micstandSpawnPoints;
        public List<Transform> stoodSpawnPoints;
        public List<Transform> bottleSpawnPoints;

        private List<GameObject> instantiatedGameobjects;

        private void Awake() {
            EventManager.AddListener<ObjectRepairedEvent>(OnObjectRepaired);
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
            
            // Bottle spawn
            for (int i = 0; i < 4; i++) {
                transform = bottleSpawnPoints[Random.Range(0, bottleSpawnPoints.Count - 1)];
                bottleSpawnPoints.Remove(transform);
                g = Instantiate(bottlePrefab, transform);
                instantiatedGameobjects.Add(g);
            }
        }

        private void Update() {
            
        }

        private void OnObjectRepaired(ObjectRepairedEvent evt) {
            GameObject g = instantiatedGameobjects.Find(obj => obj == evt.Object);
            if (g.name.Contains("Bottle")) {
                instantiatedGameobjects.Remove(g);
                Destroy(g);
            } else {
                g.tag = "Repaired";
                g.transform.GetChild(1).gameObject.SetActive(false);
            }

            if (instantiatedGameobjects.All(obj => obj.tag.Equals("Repaired"))) {
                MinigameFinishedEvent minigameFinishedEvent = Events.MinigameFinishedEvent;
                Engagement engagement = FindObjectOfType<Engagement>();
                minigameFinishedEvent.Engagement = engagement.engagement;
                EventManager.Broadcast(minigameFinishedEvent);
            }
        }

        private void OnDestroy() {
            EventManager.RemoveListener<ObjectRepairedEvent>(OnObjectRepaired);
        }
    }
}