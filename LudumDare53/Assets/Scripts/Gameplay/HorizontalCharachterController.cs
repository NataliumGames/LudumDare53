using System;
using Game;
using Game.Managers;
using UnityEngine;

namespace Gameplay {
    
    [RequireComponent(typeof(Rigidbody))]
    public class HorizontalCharachterController : MonoBehaviour {

        public float velocity = 5f;
        public float jumpForce = 10f;
        private float gravity = 14f;
        private float verticalVelocity;
        private bool isGrounded = true;
        private int score = 0;
        private Rigidbody _rigidbody;

        private void Start() {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate() {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
                _rigidbody.velocity = new Vector3(0, 5, 0) * velocity;
                isGrounded = false;
            }
            
            float moveHorizontal = Input.GetAxis("Horizontal");
            Vector3 movement = new Vector3(moveHorizontal, 0f, 0f);
            _rigidbody.velocity = movement * velocity;
        }

        private void IncrementScore() {
            score++;
            WallPassedEvent wallPassedEvent = Events.WallPassedEvent;
            wallPassedEvent.Score = score;
            EventManager.Broadcast(wallPassedEvent);
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.CompareTag("Ground")) {
                isGrounded = true;
            }
        }

        private void OnTriggerEnter(Collider other) {
            IncrementScore();
        }
    }
}