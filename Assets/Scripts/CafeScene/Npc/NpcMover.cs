using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum MoveMode
{
    IDLE,
    WALKING,
    SITTING,
    EATING,
}


// NPC 상태 enum
public enum NpcState
{
    FINDING_TABLE,
    WALKING_TO_QUEUE,
    WAITING_IN_QUEUE,
    WALKING_TO_TABLE,
    SITTING,
    WAITING_FOR_ORDER,
    EATING,
    LEAVING
}

public class NpcMover : MonoBehaviour
{
    public PlayerItem MenuToOrder = PlayerItem.NONE;
    public Transform SpawnPosition; // 스폰위치와 퇴장위치가 동일함

    public Image SpeechBubbleImage;
    public Text SpeechBubbleText;

    // 대기열 위치로 이동 (Scene에 Queue Manager 오브젝트가 있다고 가정)
    public NpcQueueManager npcQueueManager;

    // 내부 상태 관리용 변수들
    protected Animator animator;
    protected float moveSpeed = 3f; // NPC 이동 속도
    float RADIUS_BETWEEN_TABLE_AND_NPC = 1.7f;

    protected TableAndChairs targetTableObject;
    protected Transform sitPosition;
    protected bool isOrderDelivered = false;
    protected MoveMode moveMode = MoveMode.IDLE;
    protected MoveDirection moveDirection = MoveDirection.IDLE;

    [SerializeField]
    protected NpcState currentState;

    protected void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator component not found on NPC!");
        }
        SpawnPosition = CafeSceneManager.Instance.npcSpawnPosition;

        currentState = NpcState.FINDING_TABLE;
        npcQueueManager = NpcQueueManager.Instance;
        StartCoroutine(StateMachine());
    }

    // NPC의 상태 머신 코루틴
    protected IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case NpcState.FINDING_TABLE:
                    yield return Coroutine_FindEmptyTable();
                    break;

                case NpcState.WALKING_TO_QUEUE:
                    yield return Coroutine_WalkToQueue();
                    break;

                case NpcState.WAITING_IN_QUEUE:
                    yield return Coroutine_WaitInQueue();
                    break;

                case NpcState.WALKING_TO_TABLE:
                    yield return Coroutine_WalkToTable();
                    break;

                case NpcState.SITTING:
                    yield return Coroutine_Sit();
                    break;

                case NpcState.WAITING_FOR_ORDER:
                    yield return Coroutine_WaitForOrder();
                    break;

                case NpcState.EATING:
                    yield return Coroutine_Eat();
                    break;

                case NpcState.LEAVING:
                    yield return Coroutine_Leave();
                    break;
            }

            yield return null;
        }
    }

    // 상태별 코루틴 구현
    protected IEnumerator Coroutine_FindEmptyTable()
    {
        Debug.Log("NPC: Finding empty table...");
        int grantedEmptyTableIndex = TableManager.Instance.MarkEmptyTable();
        if (grantedEmptyTableIndex != -1)
        {
            // emptyTableAndChairs = tableManager.tables[grantedEmptyTableIndex].gameObject;
            targetTableObject = TableManager.Instance.GetTable(grantedEmptyTableIndex);
            currentState = NpcState.WALKING_TO_TABLE;
            yield break; // 빈 테이블 찾으면 바로 이동
        }
        else
        {
            // 빈 테이블이 없으면 대기열로 이동
            Debug.Log("No empty tables, moving to queue...");
            currentState = NpcState.WALKING_TO_QUEUE;
            yield break;
        }
    }

    protected IEnumerator Coroutine_WalkToQueue()
    {
        Debug.Log("NPC: Walking to queue...");
        // 대기열에 사람 수에 따라 위치 계산
        Vector3 destPosition = NpcQueueManager.Instance.npcQueueTailPosition; ;
        yield return MoveToPosition(destPosition);
        NpcQueueManager.Instance.PushQueue(this);
        currentState = NpcState.WAITING_IN_QUEUE;
    }

    protected IEnumerator Coroutine_WaitInQueue()
    {
        Debug.Log("NPC: Waiting in queue...");

        // 큐의 첫 번째 NPC가 Coroutine_자신이고 빈 테이블이 생기면 테이블로 이동
        while (NpcQueueManager.Instance.GetQueueCount() > 0 && NpcQueueManager.Instance.PeekQueue() == this)
        {
            int emptyTableIndex = TableManager.Instance.MarkEmptyTable();
            if (emptyTableIndex != -1)
            {
                // 대기열에서 스스로를 pop
                NpcQueueManager.Instance.PopQueue();

                // 테이블 할당
                targetTableObject = TableManager.Instance.GetTable(emptyTableIndex);
                currentState = NpcState.WALKING_TO_TABLE;
                yield break;
            }
            yield return new WaitForSeconds(1f); // 1초마다 체크
        }

        // 아직 차례가 아니면 계속 대기
        yield return new WaitForSeconds(0.5f);
    }

    protected IEnumerator Coroutine_WalkToTable() // 전제: targetTableObject != null
    {
        Debug.Log("NPC: Walking to table...");
        if (targetTableObject == null)
        {
            Debug.LogWarning("This is Impossible - Target table object is null!");
            currentState = NpcState.WALKING_TO_QUEUE;
            yield break; // 테이블이 없으면 종료
        }


        // NPC와 targetTableObject 간의 거리 계산
        yield return MoveToPosition(targetTableObject.transform.position, 0.1f, RADIUS_BETWEEN_TABLE_AND_NPC);

        float distanceToTable = Vector3.Distance(transform.position, targetTableObject.transform.position);
        Debug.Log("NPC: Reached enouth to the table. Distance: " + distanceToTable);
        // currentState를 Sit Position으로 변경
        currentState = NpcState.SITTING;
    }

    protected IEnumerator Coroutine_Sit()
    {
        Debug.Log("NPC: Sitting...");

        // 빈 의자 찾기 (왼쪽 또는 오른쪽)
        if (targetTableObject.sitter[0] == null)
        {
            sitPosition = targetTableObject.GetChairSitPosition(0);
            if (sitPosition == null)
            {
                Debug.LogWarning("This is Impossible - No empty left chair found at the table!");
            }
        }
        else if (targetTableObject.sitter[1] == null)
        {
            sitPosition = targetTableObject.GetChairSitPosition(1);
            if (sitPosition == null)
            {
                Debug.LogWarning("This is Impossible - No empty right chair found at the table!");
            }
        }
        else
        {
            Debug.LogWarning("This is Impossible - No empty chair found at the table!");
        }
        // 앉는 위치로 이동
        transform.position = sitPosition.position;

        Debug.Log("NPC: Sitting at position: " + sitPosition.position);
        // 앉는 애니메이션 재생 (있다면)
        if (animator != null)
        {
            moveMode = MoveMode.SITTING;
            if (targetTableObject.sitter[0] == null)
            { // 왼쪽의자가 비었으면 왼쪽의자부터 앉는다.
                targetTableObject.sitter[0] = this;
                moveDirection = MoveDirection.RIGHT;
            }
            else if (targetTableObject.sitter[1] == null)
            {
                targetTableObject.sitter[1] = this;
                moveDirection = MoveDirection.LEFT;
            }
            else
            {
                Debug.LogWarning("This is Impossible - Reached Granted Table but No empty seats at table!");
                targetTableObject = null;
                currentState = NpcState.WALKING_TO_QUEUE;
                yield return null;
            }
            UpdateAnimator();
            // 3초 후 주문
            yield return new WaitForSeconds(3f);
            currentState = NpcState.WAITING_FOR_ORDER;
        }
    }

    protected IEnumerator Coroutine_WaitForOrder()
    {
        Debug.Log("NPC: Waiting for order...");

        // 무작위 메뉴 선택
        int randomMenu = UnityEngine.Random.Range(1, 4); // 1=WATER, 2=ESPRESSO, 3=AMERICANO
        switch (randomMenu)
        {
            case 1:
                MenuToOrder = PlayerItem.WATER;
                break;
            case 2:
                MenuToOrder = PlayerItem.ESPRESSO;
                break;
            case 3:
                MenuToOrder = PlayerItem.AMERICANO;
                break;
        }

        Debug.Log("NPC ordered: " + MenuToOrder);
        SpeakBubbleActive("Order: " + MenuToOrder.ToString(), -1f);

        // 주문 전달 대기
        while (!isOrderDelivered)
        {
            yield return new WaitForSeconds(0.5f);
        }

        currentState = NpcState.EATING;
    }

    public void SpeakBubbleActive(string speech, float seconds)
    {
        StartCoroutine(SpeakBubbleCoroutine(speech, seconds));
    }

    protected IEnumerator SpeakBubbleCoroutine(string speech, float seconds = -1f)
    {
        SpeechBubbleImage.gameObject.SetActive(true);
        SpeechBubbleText.text = speech;
        if (seconds != -1f)
        { // seconds가 -1이 아니면 지정된 시간 동안 표시
            yield return new WaitForSeconds(seconds);
            SpeechBubbleImage.gameObject.SetActive(false);
        }
    }

    protected IEnumerator Coroutine_Eat()
    {
        Debug.Log("NPC: Eating/Drinking...");

        SpeakBubbleActive("That's what I wanted!", 3f);
        moveMode = MoveMode.EATING;
        if (animator != null)
        {
            UpdateAnimator();
            print("Eating Animation Triggered");
        }
        else
        {
            Debug.LogWarning("Animator component not found on NPC!");
        }

        // 5초 후 떠남
        yield return new WaitForSeconds(5f);
        currentState = NpcState.LEAVING;
    }

    protected IEnumerator Coroutine_Leave()
    {
        Debug.Log("NPC: Leaving...");
        targetTableObject.isTableOccupied = false; // 테이블 사용 중 상태 해제

        // 일어서는 애니메이션 재생 (있다면)
        // if (animator != null)
        // {
        //     animator.SetBool("IsSitting", false);
        // }
        // 걷는애니메이션으로 변경
        moveMode = MoveMode.WALKING;
        UpdateAnimator();
        SpeakBubbleActive("Thanks for the meal!", 3f);

        if (targetTableObject == null)
        {
            Debug.LogWarning("This is Impossible - targetTableObject is null when leaving!");
            yield break; // 테이블이 없으면 종료
        }
        // 테이블 빈 상태로 만들기
        else
        {
            if (targetTableObject.sitter[0] == this)
            {
                targetTableObject.sitter[0] = null;
            }
            else if (targetTableObject.sitter[1] == this)
            {
                targetTableObject.sitter[1] = null;
            }
            else
            {
                Debug.LogWarning("This is Impossible - targetTableObject.sitter[] does not contain this NPC!");
            }

            if (SpawnPosition != null)
            {
                yield return MoveToPosition(SpawnPosition.transform.position);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("SpawnPosition is null!");
            }
        }
    }

    // 특정 위치로 이동하는 메서드 (x축과 y축 이동 분리)
    protected IEnumerator MoveToPosition(Vector3 position, float stopRadiusX = 0.1f, float stopRadiusY = 0.1f) // stopRadius 반경에 다다르면 종료함. 기본값은 0.1f
    {
        moveMode = MoveMode.WALKING;

        // 먼저 x축 이동
        while (Mathf.Abs(transform.position.x - position.x) > stopRadiusX)
        {
            Vector3 direction = new Vector3(position.x - transform.position.x, 0, 0).normalized;
            MoveInDirection(direction);
            yield return null;
        }

        // 그 다음 y축 이동
        while (Mathf.Abs(transform.position.y - position.y) > stopRadiusY)
        {
            Vector3 direction = new Vector3(0, position.y - transform.position.y, 0).normalized;
            MoveInDirection(direction);
            yield return null;
        }

        moveMode = MoveMode.IDLE;
        moveDirection = MoveDirection.UP;
        UpdateAnimator();
    }

    // 방향에 따라 이동 및 애니메이션 처리
    protected void MoveInDirection(Vector3 direction)
    {
        // 이동 방향 설정
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0) moveDirection = MoveDirection.RIGHT;
            else moveDirection = MoveDirection.LEFT;
        }
        else
        {
            if (direction.y > 0) moveDirection = MoveDirection.UP;
            else moveDirection = MoveDirection.DOWN;
        }

        // 실제 이동
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // 애니메이션 업데이트
        UpdateAnimator();
    }

    // 애니메이터 상태 업데이트
    protected void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetInteger("MoveMode", (int)moveMode);
            animator.SetInteger("MoveDirection", (int)moveDirection);
        }
    }

    // 주문 배달 처리(TableAndChairs.cs에서 호출됨)
    public void DeliverOrder(PlayerItem deliveredItem)
    {
        if (deliveredItem == MenuToOrder)
        {
            isOrderDelivered = true;
            Debug.Log("Order delivered successfully: " + deliveredItem);
        }
        else
        {
            Debug.Log("Wrong order delivered! Expected: " + MenuToOrder + ", Got: " + deliveredItem);
        }
    }

    // 주문 정보 얻기 (외부에서 조회용)
    public PlayerItem GetOrder()
    {
        return MenuToOrder;
    }

    // NPC 상태 확인 (테이블에 앉아있는지 등)
    public bool IsWaitingForOrder()
    {
        return currentState == NpcState.WAITING_FOR_ORDER;
    }

    // 특정 테이블에 이 NPC가 앉아있는지 확인
    public bool IsSeatedAt(TableAndChairs table)
    {
        return this == table.sitter[0] || this == table.sitter[1];
    }
}