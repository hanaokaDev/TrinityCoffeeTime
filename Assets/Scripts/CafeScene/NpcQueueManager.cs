using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events; // for UnityAction

public class NpcQueueManager : MonoBehaviour
{
    public static NpcQueueManager Instance;
    
    private void Awake()
    {
        Instance = this;
    }

}
