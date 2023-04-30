using System;
using Game;
using Game.Managers;
using UnityEngine;

namespace Gameplay {
    
    [RequireComponent(typeof(CharacterController))]
    public class CharacterController3D : MonoBehaviour {

        public float speed = 5f;
        private CharacterController _characterController;

        private void Start() {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update() {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 moveDirection = new Vector3(horizontal, 0f, vertical);
            _characterController.Move(moveDirection * speed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision other) {
            if (other.transform.CompareTag("Enemy")) {
                Debug.Log("Hitted");
                HittedByEnemyEvent hittedByEnemyEvent = Events.HittedByEnemyEvent;
                EventManager.Broadcast(hittedByEnemyEvent);
            }
        }
    }
}