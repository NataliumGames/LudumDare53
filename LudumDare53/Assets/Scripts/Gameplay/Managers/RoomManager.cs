using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Managers {
    
    public class RoomManager : MonoBehaviour {

        public float spawnDelay = 3f;
        public Transform spawnPos;
        public GameObject wall;

        private List<GameObject> instantiatedWalls;

        private void Start() {
            instantiatedWalls = new List<GameObject>();
            
            InvokeRepeating("SpawnWall", 0f, spawnDelay);
        }

        private void SpawnWall() {
            Vector3 pos = new Vector3(Random.Range(-2.4f, 1.8f), spawnPos.position.y, spawnPos.position.z);
            GameObject wall = Instantiate(this.wall, pos, Quaternion.identity);
            wall.GetComponent<WallMover>().StartMovement();
            instantiatedWalls.Add(wall);
        }
    }
}