using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngagementBar : MonoBehaviour
{
    public float decrease = 0.02f;
    public float current = 1.0f; // start from max bar
    public float pointValue = 0.02f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (current > 1.0f)
        {
            current = 1.0f;
        }

        if (transform.localScale.y > 0.0f)
        {
            Vector3 p = transform.localPosition;
            Vector3 s = transform.localScale;

            /*transform.localPosition = Vector3.Lerp(p, new Vector3(-8f + points / 2, p.y, p.z), 15 * Time.deltaTime);
            transform.localScale = Vector3.Lerp(s, new Vector3(s.x, points, s.z), 15 * Time.deltaTime);*/

            current -= decrease * Time.deltaTime;
            if (current < 0.0f)
                current = 0.0f;

            transform.localPosition = Vector3.Lerp(p, new Vector3(-8f + current * 8, p.y, p.z), 15 * Time.deltaTime);
            transform.localScale = Vector3.Lerp(s, new Vector3(s.x, current * 16, s.z), 15 * Time.deltaTime);
        }
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
