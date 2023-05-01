using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EngagementBar : MonoBehaviour
{
    public GameObject engagementBarGameObject;

    public float decrease = 0.02f;
    public float initValue = 1.0f;
    public float bonusValue = 0.025f;
    public float malusValue = -0.05f;

    private Image imageBar;

    private bool isRunning = false;
    private float current;

    // Start is called before the first frame update
    void Start()
    {
        imageBar = engagementBarGameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            if (current > 1.1f)
            {
                current = 1.1f;
            }

            if (current > 0.0f)
            {
                current -= decrease * Time.deltaTime;
            }
            if (current < 0.0f)
                current = 0.0f;

            imageBar.fillAmount = current;
        }
    }

    public float GetCurrent()
    {
        return current;
    }

    public void AddPoints(float pts)
    {
        current += pts;
    }

    public void AddBonus()
    {
        current += bonusValue;
    }

    public void AddMalus()
    {
        current += malusValue;
    }

    public void StartEngagementBar(float initVal)
    {
        current = initValue;
        isRunning = true;
    }

    public float StopEngagementBar()
    {
        isRunning = false;
        return current;
    }
}
