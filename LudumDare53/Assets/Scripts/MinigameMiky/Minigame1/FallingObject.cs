using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public Vector3 customGravity;
    public Vector3 rotation;

    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation * Time.deltaTime); // The main part of the rotation script.

        if (transform.position.y < -10.0f)
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        rb.velocity += customGravity * Time.fixedDeltaTime;
    }
}
