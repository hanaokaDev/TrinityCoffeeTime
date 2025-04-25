
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableItem : MonoBehaviour
{
    [SerializeField]
    public Sprite nextUseButtonSprite;
    // private SpriteRenderer spriteRenderer; // TODO: Highlighting 효과

    public ProgressBar progressBar;


    public int maxTime;


    void Awake()
    {
        // myText = GetComponent<Text>();
        // mySlider = GetComponent<Slider>();
    }

    protected virtual void Start()
    {
        progressBar.SetMaxValue(maxTime);
        progressBar.SetValue(0);

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
        else{
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        var character = collision.GetComponent<PlayerMover>();
        if(character != null && character.isOwned)
        {
            // spriteRenderer.material.SetFloat("_Highlighted", 0f); // TODO: Highlighting 효과
            HudManager.Instance.UnsetUseButton();
        }
        else{
        }
    }
    
    public virtual void OnClickInteractableGameObject()
    {
        float currentValue = progressBar.GetValue();
        progressBar.SetValue(currentValue + 1);
    }

}
