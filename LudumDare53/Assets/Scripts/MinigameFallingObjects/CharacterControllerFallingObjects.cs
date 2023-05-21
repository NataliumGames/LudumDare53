using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerFallingObjects : MonoBehaviour
{
    public float speed;

    private Rigidbody rb;
    private FloatingJoystick floatingJoystick;

    public delegate void TriggerCallback(float points);
    private TriggerCallback callback;

    private bool gameRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        floatingJoystick = FindObjectOfType<FloatingJoystick>();

        // Mobile controls
        floatingJoystick.gameObject.SetActive(SystemInfo.deviceType == DeviceType.Handheld);
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameRunning)
        {
            float moveHorizontal;

            if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                moveHorizontal = Input.GetAxis("Horizontal");
            }
            else
            {
                moveHorizontal = floatingJoystick.Horizontal;
            }

            rb.velocity = new Vector3(moveHorizontal * speed, -5.0f, 0.0f);
        }
    }

    public void SetCollisionCallback(TriggerCallback callback)
    {
        this.callback = callback;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Tomato"))
        {
            callback(-1.0f);
        }
        else
        {
            callback(1.0f);
        }

        Destroy(other.gameObject);

    }

    public void SetGameRunning(bool gameOver)
    {
        this.gameRunning = gameOver;
    }
}
