using System;
using Game;
using Game.Managers;
using UnityEngine;

namespace Gameplay {
    
    [RequireComponent(typeof(Rigidbody))]
    public class HorizontalCharachterController : MonoBehaviour {

        public float speed = 5f;
        public float jumpForce = 10f;
        private bool isGrounded = true;
        private int score = 0;
        private float yVelocity;
        private Vector3 direction, velocity;
        private Rigidbody _rigidbody;
        private CharacterController _characterController;

        private void Start() {
            _rigidbody = GetComponent<Rigidbody>();
            _characterController = GetComponent<CharacterController>();
        }

        private void Update() {
            // float moveHorizontal = Input.GetAxis("Horizontal");
            //
            // if(moveHorizontal != 0 && isGrounded) {
            //     Vector3 movement = new Vector3(moveHorizontal, 0f, 0f);
            //     _rigidbody.velocity = movement * velocity;
            // }
            //
            // if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            //     isGrounded = false;
            //     _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            // }

            // float horizontal = Input.GetAxis("Horizontal");
            // Vector3 moveDirection = new Vector3(horizontal, 0f, 0f);
            // _characterController.Move(moveDirection * velocity * Time.deltaTime);

            var horizontal = Input.GetAxis("Horizontal");
            if (isGrounded) {
                
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