using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonSmasher : MonoBehaviour
{
    public GameObject textGameObject;

    private int counter = 0;
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = textGameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            counter++;
            text.text = "Counter: " + counter;
        }

    }
}
