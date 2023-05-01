using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UI;


public class ButtonSmasher : MonoBehaviour
{

    public int counter = 0;
    public GameObject gaugeBarObject;

    public int maxCounterValue;

    public GameObject multiplierTextObject;

    private int counterStep;

    private GaugeBar gaugeBar;
    private TextMeshProUGUI multiplierText;

    // Start is called before the first frame update
    void Start()
    {
        gaugeBar = gaugeBarObject.GetComponent<GaugeBar>();
        multiplierText = multiplierTextObject.GetComponent<TextMeshProUGUI>();

        counterStep = (int)maxCounterValue/5;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            counter++;
            gaugeBar.IncrementValueBy(1.0f/maxCounterValue);

            int multiplier = updateMultiplier();
            multiplierText.SetText("x"+multiplier);
        }
    }

    private int updateMultiplier() {
        int multiplier = 0;
        if (counter >= counterStep) {
            multiplier = 1;
        }
        if (counter >= counterStep*2) {
            multiplier = 2;
        }
        if (counter >= counterStep*3) {
            multiplier = 4;
        }
        if (counter >= counterStep*4) {
            multiplier = 8;
        }
        if (counter >= counterStep*5) {
            multiplier = 10;
        }

        return multiplier;
    }
}
