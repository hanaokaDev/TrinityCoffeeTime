
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EspressoMachine : InteractableItem
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

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }

    public override void OnClickInteractableGameObject()
    {
        Debug.Log("Espresso Machine Clicked");
        base.OnClickInteractableGameObject();
    }

}


public enum EspressoType
{
    NONE,
    ESPRESSO,
    AMERICANO,
    CARAMEL_MACCHIATO,
    CAPPUCCINO,
    LATTE,
    MOCHA,
    MACCHIATO,
    CARAMEL_LATTE,
    VANILLA_LATTE
}
