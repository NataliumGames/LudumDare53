using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpacebarUI : MonoBehaviour
{

    public Sprite IdleSpacebar;
    public Sprite PressedSpacebar;

    private bool wasPressed = false;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = transform.gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (wasPressed) {return;}

            transform.gameObject.GetComponent<Image>().sprite = PressedSpacebar;
        }
        else if (Input.GetKeyUp("space")) {

            transform.gameObject.GetComponent<Image>().sprite = IdleSpacebar;
        }
    }
}
