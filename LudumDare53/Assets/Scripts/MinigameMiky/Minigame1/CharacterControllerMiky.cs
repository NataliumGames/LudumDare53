using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerMiky : MonoBehaviour
{
    public float speed;

    public float points = 1.0f;

    private Rigidbody rb;

    public delegate void TriggerCallback();
    private TriggerCallback callback;

    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        points = 1.0f;

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameOver)
        {

            float moveHorizontal = Input.GetAxis("Horizontal");
            //float moveVertical = Input.GetAxis("Vertical");

            rb.velocity = new Vector3(moveHorizontal * speed, -5.0f, 0.0f);
        }
    }

    public void SetCollisionCallback(TriggerCallback callback)
    {
        this.callback = callback;
    }

    private void OnTriggerEnter(Collider other)
    {
        //points += 1.0f;
        Destroy(other.gameObject);

        callback();
    }

    public void SetGameOver(bool gameOver)
    {
        this.gameOver = gameOver;
    }
}
