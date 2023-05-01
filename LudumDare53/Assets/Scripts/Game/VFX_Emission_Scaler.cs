using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Emission_Scaler : MonoBehaviour
{

    public ButtonSmasher buttonSmasher;

    public GameObject[] VFX_objects;

    public int[] multiplierCounterThresholds;

    public int maxParticleEmission;

    private float emissionStep;

    private ParticleSystem[] VFX_ParticleSystems;
    // Start is called before the first frame update
    void Start()
    {   
        VFX_ParticleSystems = new ParticleSystem[VFX_objects.Length];

        for (int i = 0; i < VFX_objects.Length; i++)
        {
            VFX_ParticleSystems[i] = VFX_objects[i].GetComponent<ParticleSystem>();
        }

        emissionStep = maxParticleEmission/multiplierCounterThresholds.Length;

        foreach (var vfx in VFX_ParticleSystems)
        {
            var emission = vfx.emission;
            emission.rateOverTime = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonSmasher == null || VFX_objects == null || multiplierCounterThresholds == null) {
            return;
        }

        foreach (var vfx in VFX_ParticleSystems)
        {
            var emission = vfx.emission;
            for (int i = 0; i < multiplierCounterThresholds.Length; i++)
            {   
                if (i+1 == multiplierCounterThresholds.Length) {
                    if (buttonSmasher.counter > multiplierCounterThresholds[i]) {
                        emission.rateOverTime = emissionStep*i + 20;
                    }
                }
                else {
                    if (buttonSmasher.counter > multiplierCounterThresholds[i] && buttonSmasher.counter < multiplierCounterThresholds[i+1]) {
                        emission.rateOverTime = emissionStep*i + 20;
                    }
                }
            }
        }
    }
}
