
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableItem : MonoBehaviour
{
    [SerializeField]
    public Sprite nextUseButtonSprite; // TODO: 가구별로 nextUseButtonSprite 의 아이콘을 다르게 Inspector에서 할당할 수 있음.
    // private SpriteRenderer spriteRenderer; // TODO: Highlighting 효과

    public ProgressBar progressBar;

    [SerializeField]
    protected PlayerMover chosenPlayer;


    public int maxTime;

    // progressBar의 GetValue() 결과를 가져옴
    public int currentTime 
    { 
        get { return (int)progressBar.GetValue(); } 
    }


    void Awake()
    {
        // myText = GetComponent<Text>();
        // mySlider = GetComponent<Slider>();
    }

    protected virtual void Start()
    {
        progressBar?.SetMaxValue(maxTime);
        progressBar?.SetValue(0);
        // TODO: Highlighting 효과
        // spriteRenderer = GetComponent<SpriteRenderer>();
        // var inst = Instantiate(spriteRenderer.material);
        // spriteRenderer.material = inst;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.GetComponent<PlayerMover>();
        if(character != null && character.isOwned)
        {
            // spriteRenderer.material.SetFloat("_Highlighted", 1f); // TODO: Highlighting 효과
            HudManager.Instance.SetUseButton(nextUseButtonSprite, OnClickInteractableGameObject);
        }
        chosenPlayer = character;
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        var character = collision.GetComponent<PlayerMover>();
        if(character != null && character.isOwned)
        {
            // spriteRenderer.material.SetFloat("_Highlighted", 0f); // TODO: Highlighting 효과
            HudManager.Instance.UnsetUseButton();
        }
        chosenPlayer = null;
    }
    
    public virtual void OnClickInteractableGameObject()
    {
        float currentValue = progressBar.GetValue();
        progressBar.SetValue(currentValue + 1);
    }

}
