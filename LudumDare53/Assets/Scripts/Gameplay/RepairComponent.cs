using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Managers;
using Managers;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Gameplay {
    public class RepairComponent : MonoBehaviour {
        
        public float bonus = 0.1f;

        private GameObject _canvas;
        private GameObject text;
        private GameObject gaugeBarGameObject;
        private GaugeBar gaugeBar;
        private GameObject nearGameobject;
        private bool canvasVisibility = false;
        private bool repairing = false;
        private Engagement _engagement;
        private AudioManager _audioManager;
        private Dictionary<String, float> gameobjectMap;

        private RepairButton buttonRepair;

        private void Start() {
            _audioManager = FindObjectOfType<AudioManager>();
            _engagement = FindObjectOfType<Engagement>();
            _canvas = transform.GetChild(1).gameObject;
            text = _canvas.transform.GetChild(0).gameObject;
            gaugeBarGameObject = _canvas.transform.GetChild(1).gameObject;
            gaugeBar = gaugeBarGameObject.GetComponent<GaugeBar>();
            gameobjectMap = new Dictionary<String, float>();

            buttonRepair = FindObjectOfType<RepairButton>(true);
        }

        private void Update() {
            if (canvasVisibility && nearGameobject && !repairing) {
                if (Input.GetKeyDown(KeyCode.E) || buttonRepair.isPressed) {
                    BeginRepair(nearGameobject);
                }
            }
        }

        private void BeginRepair(GameObject obj) {
            repairing = true;
            text.SetActive(false);
            gaugeBarGameObject.SetActive(true);

            gameobjectMap.TryAdd(obj.name, 0f);

            StartCoroutine(Repair(obj));
        }

        private IEnumerator Repair(GameObject obj) {
            bool running = true;
            
            while (running && repairing) {
                if (gameobjectMap[obj.name] >= 1f) {
                    running = false;
                    repairing = false;
                    gaugeBar.SetBarValue(0f);
                    gaugeBarGameObject.SetActive(false);
                    
                    BroadcastRepairedEvent(obj);
                } else {
                    gameobjectMap[obj.name] += bonus;
                    gaugeBar.SetBarValue(gameobjectMap[obj.name]);
                    _engagement.IncrementValueBy(bonus);
                    _audioManager.PlayFX("Build");
                }

                yield return new WaitForSeconds(1f);
            }
        }

        private void BroadcastRepairedEvent(GameObject obj) {
            ObjectRepairedEvent objectRepairedEvent = Events.ObjectRepairedEvent;
            objectRepairedEvent.Object = nearGameobject;
            EventManager.Broadcast(objectRepairedEvent);
        }
        
        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Repairable")) {
                canvasVisibility = true;
                text.SetActive(canvasVisibility);
                nearGameobject = other.gameObject;
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Repairable")) {
                repairing = false;
                canvasVisibility = false;
                text.SetActive(canvasVisibility);
                gaugeBarGameObject.SetActive(canvasVisibility);
                nearGameobject = null;
            }
        }
    }
}