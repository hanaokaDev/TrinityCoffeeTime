
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EspressoMachine : MonoBehaviour
{
    [SerializeField]
    public Sprite nextUseButtonSprite;
    // private SpriteRenderer spriteRenderer; // TODO: Highlighting 효과

    public enum State { IDLE, BUSY, DONE, DIRTY};
    public State currentState;

    public ProgressBar progressBar;

    int timeLeft = 0;
    public int brewTime; // 초 단위로 설정

    Text myText;
    Slider mySlider;

    void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    void Start()
    {
        currentState = State.IDLE;
        progressBar.SetMaxValue(brewTime);
        progressBar.SetValue(0);

        // TODO: Highlighting 효과
        // spriteRenderer = GetComponent<SpriteRenderer>();
        // var inst = Instantiate(spriteRenderer.material);
        // spriteRenderer.material = inst;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Espresso Machine Triggered");
        var character = collision.GetComponent<PlayerMover>();
        if(character == null) {
            Debug.Log("Espresso Machine Triggered but character is null");
        }

        if(character != null && character.isOwned)
        {
            // spriteRenderer.material.SetFloat("_Highlighted", 1f); // TODO: Highlighting 효과
            HudManager.Instance.SetUseButton(nextUseButtonSprite, OnClickEspressoMachine);
        }
        else{
            Debug.Log("Espresso Machine Triggered but not owned");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Espresso Machine Exitted");
        var character = collision.GetComponent<PlayerMover>();
        if(character != null && character.isOwned)
        {
            // spriteRenderer.material.SetFloat("_Highlighted", 0f); // TODO: Highlighting 효과
            HudManager.Instance.UnsetUseButton();
        }
        else{
            Debug.Log("Espresso Machine Exitted but not owned");
        }
    }
    
    public void OnClickEspressoMachine()
    {
        Debug.Log("Espresso Machine Clicked");
        float currentValue = progressBar.GetValue();
        progressBar.SetValue(currentValue + 1);
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
