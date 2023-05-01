using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class TestGaugeNew : MonoBehaviour
{
    private GaugeBarVertical gaugeBarVertical;
    private float val = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        gaugeBarVertical = FindObjectOfType<GaugeBarVertical>();
        gaugeBarVertical.value = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        gaugeBarVertical.DecrementValueBy(0.05f * Time.deltaTime);

        if (Input.GetKeyDown("space"))
        {
            gaugeBarVertical.IncrementValueBy(0.05f);
        }
    }
}
