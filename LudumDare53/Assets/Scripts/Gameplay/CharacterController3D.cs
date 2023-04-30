using System;
using System.Collections;
using Game;
using Game.Managers;
using UnityEngine;

namespace Gameplay {
    
    [RequireComponent(typeof(CharacterController))]
    public class CharacterController3D : MonoBehaviour {

        public float speed = 5f;
        public float invulnerabilityDuration = 3f;
        private CharacterController _characterController;
        private bool isInvulnerable = true;

        private void Start() {
            _characterController = GetComponent<CharacterController>();

            StartCoroutine(EndInvulnerability());
        }

        public IEnumerator EndInvulnerability() {
            yield return new WaitForSeconds(invulnerabilityDuration);
            isInvulnerable = false;
        }

        private void Update() {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 moveDirection = new Vector3(horizontal, 0f, vertical);
            _characterController.Move(moveDirection * speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.transform.CompareTag("Enemy") && !isInvulnerable) {
                HittedByEnemyEvent hittedByEnemyEvent = Events.HittedByEnemyEvent;
                EventManager.Broadcast(hittedByEnemyEvent);
            }
        }
    }
}