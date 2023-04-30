using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj : MonoBehaviour
{
    public Vector3 customGravity;
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10.0f)
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        rb.velocity += customGravity * Time.fixedDeltaTime;
    }
}
