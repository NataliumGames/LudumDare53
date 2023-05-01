using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class CameraShakeController : MonoBehaviour
    {


        public float shakeStrength = 1;

        private ButtonSmasher buttonSmasher;
        private VFX_Emission_Scaler vfx_Emission_Scaler;
        private CameraShake cameraShake;
        private int lastCounterThreshold;
        // Start is called before the first frame update
        void Start()
        {
            buttonSmasher = transform.gameObject.GetComponent<ButtonSmasher>();
            vfx_Emission_Scaler = transform.gameObject.GetComponent<VFX_Emission_Scaler>();
            cameraShake = transform.gameObject.GetComponent<CameraShake>();

            lastCounterThreshold = vfx_Emission_Scaler.multiplierCounterThresholds[vfx_Emission_Scaler.multiplierCounterThresholds.Length-1];
            Debug.Log(lastCounterThreshold);
        }

        // Update is called once per frame
        void Update()
        {
            float counterProgressPercentage = (float)buttonSmasher.counter/lastCounterThreshold;
            cameraShake.shakeAmount = counterProgressPercentage*shakeStrength;
        }
    }
}
