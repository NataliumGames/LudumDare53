using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    public float velocity  = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float depth = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0f, depth);
        transform.Translate(moveDirection * velocity * Time.deltaTime);

        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f));
        if (Input.GetKey(KeyCode.E))
            transform.Rotate(new Vector3(0.0f, -1.0f, 0.0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isBonus = other.name.Contains("Bonus");

        foreach(Collider c in other.transform.parent.GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }

        if (isBonus)
        {
            Debug.Log("Bonus");
        }
        else Debug.Log("Malus");
    }
}
