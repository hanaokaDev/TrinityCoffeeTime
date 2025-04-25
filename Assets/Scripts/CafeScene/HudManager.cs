using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events; // for UnityAction

public class HudManager : MonoBehaviour
{
    public static HudManager Instance;
    
    [SerializeField]
    private Button _UseButton;
    
    [SerializeField]
    private Sprite _OriginUseButtonSprite;

    private void Awake()
    {
        Instance = this;
    }

    public void SetUseButton(Sprite sprite, UnityAction action)
    {
        Debug.Log("SetUseButton called");
        _UseButton.image.sprite = sprite;
        _UseButton.onClick.AddListener(action);
        _UseButton.interactable = true;
    }
    
    public void UnsetUseButton()
    {
        Debug.Log("UnsetUseButton called");
        _UseButton.image.sprite = _OriginUseButtonSprite;
        _UseButton.onClick.RemoveAllListeners();
        _UseButton.interactable = false;
    }
}
