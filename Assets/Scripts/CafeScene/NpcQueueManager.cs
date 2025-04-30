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

    public Transform npcQueueHeadTransform;

    public Vector3 npcQueueHeadPosition{ // 대기열의 최선두 위치
        get
        {
            return npcQueueHeadTransform.position; // 대기열의 최전열 NPC의 위치를 반환
        }
    } 
    public Vector3 npcQueueTailPosition // 대기열의 최후미 위치
    {
        get
        {
            return waitingQueue.Count > 0 ? npcQueueHeadPosition + waitingQueue.Count * new Vector3(0,-1.5f,0) : npcQueueHeadPosition; // 대기열이 비어있지 않으면 최전열 NPC의 위치를 반환
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    public void PushQueue(NpcMover npc)
    {
        waitingQueue.Enqueue(npc);
        Debug.Log("NPC added to queue: " + npc.name);
    }

    public int GetQueueCount()
    {
        return waitingQueue.Count;
    }

    public NpcMover PopQueue()
    {
        if (waitingQueue.Count > 0)
        {
            NpcMover npc = waitingQueue.Dequeue();
            Debug.Log("NPC removed from queue: " + npc.name);
            return npc;
        }
        else
        {
            Debug.Log("No NPCs in queue to remove.");
            return null;
        }
    }
}
