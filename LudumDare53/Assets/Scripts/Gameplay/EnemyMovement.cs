using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Gameplay {
    
    [RequireComponent(typeof(CharacterController))]
    public class EnemyMovement : MonoBehaviour {

        public float speed = 5f;
        private CharacterController _characterController;
        private Transform targetTransform;
        
        private void Awake() {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update() {
            if (targetTransform != null) {
                Vector3 targetDirection = targetTransform.position - transform.position;
                _characterController.Move(targetDirection * speed * Time.deltaTime);
                transform.LookAt(targetTransform);
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player"))
                targetTransform = other.transform;
        }

        private void OnTriggerStay(Collider other) {
            if (other.CompareTag("Player"))
                targetTransform = other.transform;
        }

        private void OnTriggerExit(Collider other) {
            targetTransform = null;
        }
    }
}