using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EngagementBar : MonoBehaviour
{
    public float decrease = 0.02f;
    public float current = 1.0f; // start from max bar
    public float pointValue = 0.025f;

    private Image imageBar;

    // Start is called before the first frame update
    void Start()
    {
        imageBar = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (current > 1.0f)
        {
            current = 1.0f;
        }

        if (current > 0.0f)
        {
            current -= decrease * Time.deltaTime;
            if (current < 0.0f)
                current = 0.0f;
        }

        imageBar.fillAmount = current;
    }

    void AddPoints(float pts)
    {
        current += pts;
    }

    public void AddPoint()
    {
        current += pointValue;
    }
}
