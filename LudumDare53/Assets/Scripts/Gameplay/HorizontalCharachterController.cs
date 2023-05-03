using System;
using Game;
using Game.Managers;
using Managers;
using UnityEngine;

namespace Gameplay {
    
    [RequireComponent(typeof(Rigidbody))]
    public class HorizontalCharachterController : MonoBehaviour {

        public float velocity = 5f;
        public float jumpForce = 10f;
        private bool isGrounded = true;
        private int score = 0;
        private Rigidbody _rigidbody;
        private CharacterController _characterController;
        private Engagement _engagement;
        private CameraShake _cameraShake;
        private AudioManager _audioManager;

        private void Start() {
            _rigidbody = GetComponent<Rigidbody>();
            _characterController = GetComponent<CharacterController>();
            _engagement = FindObjectOfType<Engagement>();
            _cameraShake = FindObjectOfType<CameraShake>();
            _audioManager = FindObjectOfType<AudioManager>();
        }

        private void Update() {
            float moveHorizontal = Input.GetAxis("Horizontal");
            
            if(moveHorizontal != 0 && isGrounded) {
                Vector3 movement = new Vector3(moveHorizontal, 0f, 0f);
                _rigidbody.velocity = movement * velocity;
            }
            
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
                isGrounded = false;
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            float horizontal = Input.GetAxis("Horizontal");
            Vector3 moveDirection = new Vector3(horizontal, 0f, 0f);
            _characterController.Move(moveDirection * velocity * Time.deltaTime);
        }

        private void IncrementScore() {
            score++;
            WallPassedEvent wallPassedEvent = Events.WallPassedEvent;
            wallPassedEvent.Score = score;
            EventManager.Broadcast(wallPassedEvent);
        }
        
        private void DecrementScore() {
            score--;
            WallPassedEvent wallPassedEvent = Events.WallPassedEvent;
            wallPassedEvent.Score = score;
            EventManager.Broadcast(wallPassedEvent);
        }

        private void OnCollisionEnter(Collision other) {
            if (other.transform.CompareTag("Ground"))
                isGrounded = true;
        }

        private void OnTriggerEnter(Collider other) {
            bool isBonus = other.name.Contains("Bonus");

            foreach (Collider c in other.transform.parent.GetComponentsInChildren<Collider>())
            {
                c.enabled = false;
            }

            if (isBonus)
            {
                IncrementScore();
                _engagement.IncrementValueBy(0.1f);
                //Debug.Log("Bonus");
            }
            else
            {
                DecrementScore();
                _engagement.DecrementValueBy(0.1f);
                _cameraShake.Shake(0.1f);
                _audioManager.PlayDamage();
                //Debug.Log("Malus");
            }
        }
    }
}