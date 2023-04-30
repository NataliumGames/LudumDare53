using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerMiky : MonoBehaviour
{
    public float speed;
    public GameObject engBar;

    public float points = 1.0f;

    private Rigidbody rb;

    private EngagementBar engagementBar;

    // Start is called before the first frame update
    void Start()
    {
        points = 1.0f;

        engagementBar = engBar.GetComponent<EngagementBar>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(moveHorizontal * speed, -10.0f, 0.0f);

        // Try out this delta time method??
        //rb2d.transform.position += new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        //points += 1.0f;
        Destroy(other.gameObject);

        engagementBar.AddPoint();
    }
}
