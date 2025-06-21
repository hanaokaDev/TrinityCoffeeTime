using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    // 이벤트 정의
    // public delegate void GameStateChangeHandler(bool isPause);
    // public static event GameStateChangeHandler OnGameStateChanged;
    public bool isPause = false;
}
