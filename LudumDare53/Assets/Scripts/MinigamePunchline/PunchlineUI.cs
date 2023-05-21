using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PunchlineUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Sprite idleSpacebarSprite;
    public Sprite pressedSpacebarSprite;

    private Button spacebarButton;
    private Image spacebarImage;

    private Action ButtonDownCallback;
    private Action ButtonUpCallback;

    private void Start()
    {
        spacebarButton = GetComponent<Button>();
        spacebarImage = GetComponent<Image>();

        spacebarButton.enabled = SystemInfo.deviceType == DeviceType.Handheld;
    }

    public void SetButtonDownCallback(Action onButtonDown)
    {
        ButtonDownCallback = onButtonDown;
    }
    public void SetButtonUpCallback(Action onButtonUp)
    {
        ButtonUpCallback = onButtonUp;
    }

    public void Press()
    {
        spacebarImage.sprite = pressedSpacebarSprite;
    }

    public void Release()
    {
        spacebarImage.sprite = idleSpacebarSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ButtonDownCallback();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ButtonUpCallback();
    }
}
