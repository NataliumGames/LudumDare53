using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class GaugeBarVertical : MonoBehaviour {

        private Slider gaugeBar;

        public float minValue = 0.0f;
        public float maxValue = 1.0f;
        public float value = 1.0f;

        private void Awake()
        {
            if (minValue < 0.0f)
                minValue = 0.0f;
            if (maxValue > 1.0f)
                maxValue = 1.0f;

            if (value < minValue)
                value = minValue;
            if (value > maxValue)
                value = maxValue;

            gaugeBar = GetComponent<Slider>();
            gaugeBar.value = value;
        }

        private void ClipBounds()
        {
            if (value < minValue)
                value = minValue;
            if (value > maxValue)
                value = maxValue;
        }

        public void SetBarValue(float value)
        {
            this.value = value;
            ClipBounds();

            gaugeBar.value = value;
        }

        public void DecrementValueBy(float amount)
        {
            value -= amount;
            ClipBounds();

            gaugeBar.value = value;
        }
        
        public void IncrementValueBy(float amount)
        {
            value += amount;
            ClipBounds();

            gaugeBar.value = value;
        }
    }
}