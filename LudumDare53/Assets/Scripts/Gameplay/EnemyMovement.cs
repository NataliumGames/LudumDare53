using System;
using System.Collections;
using Game;
using Game.Managers;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Gameplay {
    
    [RequireComponent(typeof(CharacterController))]
    public class EnemyMovement : MonoBehaviour {

        public float speed = 5f;
        private CharacterController _characterController;
        private Transform targetTransform;
        private bool attacked = false;
        private float rotationDuration = 4f;
        
        private void Awake() {
            _characterController = GetComponent<CharacterController>();
            EventManager.AddListener<AttackEvent>(OnAttackedEvent);
        }

        private void Update() {
            if (targetTransform != null && !attacked) {
                Vector3 targetDirection = targetTransform.position - transform.position;
                _characterController.Move(targetDirection * speed * Time.deltaTime);
                transform.LookAt(targetTransform);
            }
        }

        private void OnAttackedEvent(AttackEvent evt) {
            attacked = true;
            StartCoroutine(Rotate());
        }

        private IEnumerator Rotate() {
            float elapsedTime = 0f;

            while (elapsedTime <= rotationDuration) {
                transform.Rotate(Vector3.up * 500f * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            attacked = false;
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

        private void OnDestroy() {
            EventManager.RemoveListener<AttackEvent>(OnAttackedEvent);
        }
    }
}