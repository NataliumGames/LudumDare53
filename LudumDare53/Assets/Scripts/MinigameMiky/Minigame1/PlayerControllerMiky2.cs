using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerMiky2 : MonoBehaviour
{
    public float rotationSpeed = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");

        transform.Rotate(transform.rotation.x, transform.rotation.y, moveHorizontal * rotationSpeed * Time.deltaTime);

    }
}
