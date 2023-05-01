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
        public float spawnDelay = 5f;
        public int maxEnemyAtSameTime = 5;
        private GameObject instantiatedEnemy;
        private Bounds bounds;

        private void Start() {
        }

        private void Update() {
            
        }

        public void Spawn() {
            float x = Random.Range(-12f, 12f);
            float z = Random.Range(-4f, 10f);
            instantiatedEnemy = Instantiate(enemyPrefab, new Vector3(x, 0f, z), Quaternion.identity);

            EnemySpawnedEvent enemySpawnedEvent = Events.EnemySpawnedEvent;
            enemySpawnedEvent.Enemy = instantiatedEnemy;
            EventManager.Broadcast(enemySpawnedEvent);
        }
    }
}