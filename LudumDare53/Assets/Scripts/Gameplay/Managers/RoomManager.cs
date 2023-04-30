using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Managers {
    
    public class RoomManager : MonoBehaviour {

        public float spawnDelay = 3f;
        public float increaseDifficultyTime = 10f;
        public float spawnDecreaseRate = 0.2f;
        public Transform spawnPos;
        public List<GameObject> wallPrefabs;

        private List<GameObject> instantiatedWalls;

        private void Start() {
            instantiatedWalls = new List<GameObject>();

            StartCoroutine(SpawnWall());
            StartCoroutine(IncreaseDifficulty());
        }

        private IEnumerator SpawnWall() {
            while (true) {
                Vector3 pos = new Vector3(Random.Range(-2.4f, 1.8f), spawnPos.position.y, spawnPos.position.z);
                int randomIndex = Random.Range(0, wallPrefabs.Count);
                GameObject wall = Instantiate(wallPrefabs[randomIndex], pos, Quaternion.identity);
                wall.GetComponent<WallMover>().StartMovement();
                instantiatedWalls.Add(wall);
                StartCoroutine(DestroyGameobjectAfterDelay(wall, 5f));

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
    }
}