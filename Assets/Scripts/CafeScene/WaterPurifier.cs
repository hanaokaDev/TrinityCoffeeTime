
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterPurifier : InteractableItem
{

    void Awake()
    {
        // myText = GetComponent<Text>();
        // mySlider = GetComponent<Slider>();
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void OnClickInteractableGameObject()
    {
        Debug.Log("Water Machine Clicked");
        base.OnClickInteractableGameObject();
    }

}
