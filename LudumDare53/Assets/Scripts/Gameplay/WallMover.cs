using System;
using UnityEngine;

namespace Gameplay {
    public class WallMover : MonoBehaviour {

        public float speed = 10f;
        private bool shouldMove = false;

        private void Update() {
            if(shouldMove)
                Move();
        }
        
        public void StartMovement() => shouldMove = true;

        private void Move() {
            transform.position -= Vector3.forward * speed * Time.deltaTime;
        }
    }
}