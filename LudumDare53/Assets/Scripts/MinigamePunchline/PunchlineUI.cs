using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PunchlineUI : MonoBehaviour
{
    public Sprite idleSpacebarSprite;
    public Sprite pressedSpacebarSprite;

    private Image spacebarImage;

    private void Start()
    {
        spacebarImage = GetComponent<Image>();
    }

    public void Press()
    {
        spacebarImage.sprite = pressedSpacebarSprite;
    }

    public void Release()
    {
        spacebarImage.sprite = idleSpacebarSprite;
    }
}
