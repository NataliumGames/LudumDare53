using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class GaugeBar : MonoBehaviour {

        private Slider gaugeBar;
        public float value;

        private void Awake() {
            gaugeBar = GetComponent<Slider>();
            value = gaugeBar.value;
        }

        public void SetBarValue(float value) {
            if (value >= gaugeBar.minValue && value <= gaugeBar.maxValue) {
                gaugeBar.value = value;
                this.value = gaugeBar.value;
            }
        }

        public void DecrementValueBy(float value) {
            if (value >= gaugeBar.minValue && value <= gaugeBar.maxValue) {
                gaugeBar.value -= value;
                this.value = gaugeBar.value;
            }
        }
        
        public void IncrementValueBy(float amount) {
            if (gaugeBar.value >= gaugeBar.minValue && gaugeBar.value <= gaugeBar.maxValue) {
                gaugeBar.value += amount;
                value = gaugeBar.value;
            }
        }
    }
}