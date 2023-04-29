using System;
using Game;
using Game.Managers;
using UnityEngine;

namespace Gameplay {
    
    [RequireComponent(typeof(Rigidbody))]
    public class HorizontalCharachterController : MonoBehaviour {

        public float velocity = 5f;
        public float jumpForce = 5f;
        private bool isGrounded = true;
        private int score = 0;
        private Rigidbody _rigidbody;

        private void Start() {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update() {
            float moveHorizontal = Input.GetAxis("Horizontal");
            
            if(moveHorizontal != 0 && isGrounded) {
                Vector3 movement = new Vector3(moveHorizontal, 0f, 0f);
                _rigidbody.velocity = movement * velocity;
            }
            
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        private void IncrementScore() {
            score++;
            WallPassedEvent wallPassedEvent = Events.WallPassedEvent;
            wallPassedEvent.Score = score;
            EventManager.Broadcast(wallPassedEvent);
        }

        private void OnCollisionEnter(Collision other) {
            if (other.transform.CompareTag("Ground"))
                isGrounded = true;
        }

        private void OnTriggerEnter(Collider other) {
            IncrementScore();
        }
    }
}