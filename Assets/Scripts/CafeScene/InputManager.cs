using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events; // for UnityAction

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [SerializeField]
    private InputAlloc inputAlloc = InputAlloc.PLAYER_MOVER;

    private void Awake()
    {
        Instance = this;
    }

    public InputAlloc GetInputAlloc()
    {
        return inputAlloc;
    }

    public void SetInputAlloc(InputAlloc alloc)
    {
        inputAlloc = alloc;
        Debug.Log("SetInputAlloc: " + alloc);
    }

}

public enum InputAlloc
{
    NONE,
    PLAYER_MOVER,
    SERVE_UI,
}