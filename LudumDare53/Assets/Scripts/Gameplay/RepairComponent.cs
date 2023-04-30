using System;
using System.Collections;
using Game;
using Game.Managers;
using Managers;
using TMPro;
using UI;
using UnityEngine;

namespace Gameplay {
    public class RepairComponent : MonoBehaviour {
        
        public float engagementBonus = 0.1f;

        private GameObject _canvas;
        private GameObject text;
        private GameObject gaugeBarGameObject;
        private GaugeBar gaugeBar;
        private GameObject nearGameobject;
        private bool canvasVisibility = false;
        private Engagement _engagement;
        private AudioManager _audioManager;

        private void Start() {
            _audioManager = FindObjectOfType<AudioManager>();
            _engagement = FindObjectOfType<Engagement>();
            _canvas = transform.GetChild(1).gameObject;
            text = _canvas.transform.GetChild(0).gameObject;
            gaugeBarGameObject = _canvas.transform.GetChild(1).gameObject;
            gaugeBar = gaugeBarGameObject.GetComponent<GaugeBar>();
        }

        private void Update() {
            if(Input.GetKeyDown(KeyCode.E) && canvasVisibility && nearGameobject != null)
                BeginRepair(nearGameobject);
        }

        private void BeginRepair(GameObject obj) {
            text.SetActive(false);
            gaugeBarGameObject.SetActive(true);

            StartCoroutine(FillGaugeBar());
        }

        private IEnumerator FillGaugeBar() {
            bool running = true;

            while (running) {
                if (gaugeBar.value == 1f) {
                    running = false;

                    ObjectRepairedEvent objectRepairedEvent = Events.ObjectRepairedEvent;
                    objectRepairedEvent.Object = nearGameobject;
                    EventManager.Broadcast(objectRepairedEvent);
                    
                    gaugeBar.SetBarValue(0f);
                    gaugeBarGameObject.SetActive(false);
                }
                else {
                    gaugeBar.IncrementValueBy(0.1f);
                    _engagement.IncrementValueBy(engagementBonus);
                    _audioManager.PlayFX("Build");
                }

                yield return new WaitForSeconds(1f);
            }
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
                canvasVisibility = false;
                text.SetActive(canvasVisibility);
                gaugeBarGameObject.SetActive(canvasVisibility);
                nearGameobject = null;
            }
        }
    }
}