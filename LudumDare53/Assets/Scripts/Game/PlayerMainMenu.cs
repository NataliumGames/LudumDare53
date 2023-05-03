using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            audioManager.PlayDamage();
        }
    }
}
