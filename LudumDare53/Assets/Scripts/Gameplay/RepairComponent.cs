using System;
using System.Collections;
using TMPro;
using UI;
using UnityEngine;

namespace Gameplay {
    public class RepairComponent : MonoBehaviour {
        
        private GameObject _canvas;
        private GameObject text;
        private GameObject gaugeBarGameObject;
        private GaugeBar gaugeBar;
        private GameObject nearGameobject;
        private bool canvasVisibility = false;

        private void Start() {
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
                if (gaugeBar.value == 1f)
                    running = false;
                else 
                    gaugeBar.IncrementValueBy(0.1f);

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