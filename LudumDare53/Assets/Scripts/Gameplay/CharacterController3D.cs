using System;
using System.Collections;
using Game;
using Game.Managers;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Gameplay {
    
    [RequireComponent(typeof(CharacterController))]
    public class CharacterController3D : MonoBehaviour {

        public float speed = 5f;
        public float invulnerabilityDuration = 3f;
        private CharacterController _characterController;
        private bool isInvulnerable = true;
        private GameObject enemy;
        private GameObject attackText;

        private void Start() {
            _characterController = GetComponent<CharacterController>();
            attackText = transform.GetChild(1).transform.GetChild(2).gameObject;
            
            EventManager.AddListener<EnemySpawnedEvent>(OnEnemySpawnedEvent);

            StartCoroutine(EndInvulnerability());
        }

        private void Update() {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 moveDirection = new Vector3(horizontal, 0f, vertical);
            _characterController.Move(moveDirection * speed * Time.deltaTime);
            
            if (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) <= 5f) {
                attackText.SetActive(true);

                if (Input.GetKeyDown(KeyCode.Space)) {
                    AttackEvent attackEvent = Events.AttackEvent;
                    EventManager.Broadcast(attackEvent);
                }
            } else {
                attackText.SetActive(false);
            }
        }

        public void OnEnemySpawnedEvent(EnemySpawnedEvent evt) {
            enemy = evt.Enemy;
        }
        
        public IEnumerator EndInvulnerability() {
            yield return new WaitForSeconds(invulnerabilityDuration);
            isInvulnerable = false;
        }
        
        private void OnTriggerEnter(Collider other) {
            if (other.transform.CompareTag("Enemy") && !isInvulnerable) {
                HittedByEnemyEvent hittedByEnemyEvent = Events.HittedByEnemyEvent;
                EventManager.Broadcast(hittedByEnemyEvent);
            }
        }

        private void OnDestroy() {
            EventManager.RemoveListener<EnemySpawnedEvent>(OnEnemySpawnedEvent);
        }
    }
}