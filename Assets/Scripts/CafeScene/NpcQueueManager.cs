using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events; // for UnityAction

public class NpcQueueManager : MonoBehaviour
{
    public static NpcQueueManager Instance;
    
    // 대기열 관리용 정적 변수 (모든 NPC가 공유)
    private Queue<NpcMover> waitingQueue = new Queue<NpcMover>();
    private List<GameObject> occupiedTables = new List<GameObject>();
    
    private void Awake()
    {
        Instance = this;
    }
}
