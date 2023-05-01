using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonSmasher : MonoBehaviour
{

    public int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        //text = textGameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            counter++;
            Debug.Log("Counter: " + counter);
        }

    }
}
