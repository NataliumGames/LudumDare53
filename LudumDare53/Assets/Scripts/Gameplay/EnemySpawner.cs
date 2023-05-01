using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Managers;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

namespace Gameplay {
    public class EnemySpawner : MonoBehaviour {

        public GameObject enemyPrefab;
        public MeshCollider spawnArea;
        public float spawnDelay = 5f;
        public int maxEnemyAtSameTime = 5;
        private List<GameObject> instantiatedEnemies;
        private Bounds bounds;

        private void Start() {
            instantiatedEnemies = new List<GameObject>();
            bounds = spawnArea.bounds;
        }

        private void Update() {
            
        }

        public void BeginSpawn() {
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn() {
            while (true) {
                if (instantiatedEnemies.Count != maxEnemyAtSameTime) {
                    float x = Random.Range(bounds.min.x, bounds.max.x);
                    float z = Random.Range(bounds.min.z, bounds.max.z);
                    GameObject instance = Instantiate(enemyPrefab, new Vector3(x, 0f, z), Quaternion.identity);
                    instantiatedEnemies.Add(instance);

                    EnemySpawnedEvent enemySpawnedEvent = Events.EnemySpawnedEvent;
                    enemySpawnedEvent.Enemy = instance;
                    EventManager.Broadcast(enemySpawnedEvent);
                }
                
                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }
}