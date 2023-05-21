using System;
using System.Collections;
using Game;
using Game.Managers;
using Managers;
using UI;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Gameplay {
    
    [RequireComponent(typeof(CharacterController))]
    public class CharacterController3D : MonoBehaviour {

        public float speed = 5f;
        public float invulnerabilityDuration = 3f;
        private CharacterController _characterController;
        private AudioManager _audioManager;
        private bool isInvulnerable = true;
        private bool canAttack = true;
        private GameObject enemy;
        private GameObject attackText;
        private Transform transformHead;
        private FloatingJoystick floatingJoystick;
        private Button buttonAttack;

        private void Start() {
            _characterController = GetComponent<CharacterController>();
            _audioManager = FindObjectOfType<AudioManager>();
            attackText = transform.GetChild(1).transform.GetChild(2).gameObject;
            floatingJoystick = FindObjectOfType<FloatingJoystick>();

            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                buttonAttack = floatingJoystick.transform.parent.GetChild(2).GetComponent<Button>();
                buttonAttack.onClick.AddListener(() => { Attack(); });
            }

            EventManager.AddListener<EnemySpawnedEvent>(OnEnemySpawnedEvent);

            StartCoroutine(EndInvulnerability());

            transformHead = transform.GetChild(0);
        }

        private void Update() {
            float horizontal, vertical;
            
            if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                horizontal = Input.GetAxis("Horizontal");
                vertical = Input.GetAxis("Vertical");
            }
            else
            {
                horizontal = floatingJoystick.Horizontal;
                vertical = floatingJoystick.Vertical;
            }

            Vector3 moveDirection = new Vector3(horizontal, -5f, vertical);
            _characterController.Move(moveDirection * speed * Time.deltaTime);
            moveDirection = new Vector3(horizontal, 0f, vertical);
            if(moveDirection != Vector3.zero)
                transformHead.rotation = Quaternion.LookRotation(moveDirection);
            
            if (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) <= 5f) {
                if(canAttack)
                    attackText.SetActive(true);

                if (SystemInfo.deviceType == DeviceType.Desktop && Input.GetKeyDown(KeyCode.Space)) {
                    Attack();
                }
            } else {
                attackText.SetActive(false);
            }
        }

        private void Attack() {
            if (canAttack) {
                canAttack = false;
                attackText.SetActive(false);
                AttackEvent attackEvent = Events.AttackEvent;
                EventManager.Broadcast(attackEvent);
                _audioManager.PlayPunch();
                StartCoroutine(AttackCooldown());

            }
        }

        public void OnEnemySpawnedEvent(EnemySpawnedEvent evt) {
            enemy = evt.Enemy;
        }
        
        public IEnumerator EndInvulnerability() {
            yield return new WaitForSeconds(invulnerabilityDuration);
            isInvulnerable = false;
        }

        public IEnumerator AttackCooldown() {
            yield return new WaitForSeconds(2f);
            _audioManager.PlayFXPitch("Snap", 2f);
            canAttack = true;
        }
        
        private void OnTriggerEnter(Collider other) {
            if (other.transform.CompareTag("Enemy") && !isInvulnerable) {
                isInvulnerable = true;
                HittedByEnemyEvent hittedByEnemyEvent = Events.HittedByEnemyEvent;
                EventManager.Broadcast(hittedByEnemyEvent);
                
                StartCoroutine(EndInvulnerability());
                _audioManager.PlayFX("Heckler");
            }
        }

        private void OnDestroy() {
            EventManager.RemoveListener<EnemySpawnedEvent>(OnEnemySpawnedEvent);
        }
    }
}