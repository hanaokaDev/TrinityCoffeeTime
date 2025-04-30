using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum MoveMode
{
    IDLE,
    WALKING,
    SITTING,
}

public class NpcMover : MonoBehaviour
{
    // 외부 참조용 변수들
    public GameObject EmptyTableAndChairs = null;
    public PlayerItem MenuToOrder = PlayerItem.NONE;
    public Transform SpawnPosition; // 스폰위치와 퇴장위치가 동일함

    // 대기열 위치로 이동 (Scene에 Queue Manager 오브젝트가 있다고 가정)
    public NpcQueueManager npcQueueManager;
    public TableManager tableManager;

    // 내부 상태 관리용 변수들
    private Animator animator;
    private float moveSpeed = 3f; // NPC 이동 속도
    private Vector3 targetPosition;
    private Transform sitPosition;
    private bool isOrderDelivered = false;
    private MoveMode moveMode = MoveMode.IDLE;
    private MoveDirection moveDirection = MoveDirection.IDLE;

    // NPC 상태 enum
    private enum NpcState
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
    [SerializeField]
    private NpcState currentState;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null){
            Debug.LogWarning("Animator component not found on NPC!");
        }
        SpawnPosition = CafeSceneManager.Instance.npcSpawnPosition;
        
        currentState = NpcState.FINDING_TABLE;
        npcQueueManager = NpcQueueManager.Instance;
        tableManager = TableManager.Instance;
        StartCoroutine(StateMachine());
    }

    // NPC의 상태 머신 코루틴
    private IEnumerator StateMachine()
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
    private IEnumerator Coroutine_FindEmptyTable()
    {
        Debug.Log("NPC: Finding empty table...");
        int grantedEmptyTableIndex = tableManager.MarkEmptyTable();
        if(grantedEmptyTableIndex != -1)
        {
            EmptyTableAndChairs = tableManager.tables[grantedEmptyTableIndex].gameObject;
            currentState = NpcState.WALKING_TO_TABLE;
            yield break; // 빈 테이블 찾으면 바로 이동
        }
        else{
            // 빈 테이블이 없으면 대기열로 이동
            Debug.Log("No empty tables, moving to queue...");
            currentState = NpcState.WALKING_TO_QUEUE;
            yield break;
        }
    }
    
    private IEnumerator Coroutine_WalkToQueue()
    {
        Debug.Log("NPC: Walking to queue...");
        // 대기열에 사람 수에 따라 위치 계산
        Vector3 destPosition = NpcQueueManager.Instance.npcQueueTailPosition;;            
        yield return MoveToPosition(destPosition);
        NpcQueueManager.Instance.PushQueue(this);
        currentState = NpcState.WAITING_IN_QUEUE;
    }
    
    private IEnumerator Coroutine_WaitInQueue()
    {
        Debug.Log("NPC: Waiting in queue...");
        
        // 큐의 첫 번째 NPC가 Coroutine_자신이고 빈 테이블이 생기면 테이블로 이동
        while (waitingQueue.Count > 0 && waitingQueue.Peek() == this)
        {
            // Scene에서 빈 테이블 찾기
            GameObject[] allTables = GameObject.FindGameObjectsWithTag("TableAndChairs");
            
            foreach (GameObject table in allTables)
            {
                if (IsTableEmpty(table) && !occupiedTables.Contains(table))
                {
                    // 대기열에서 제거
                    waitingQueue.Dequeue();
                    
                    // 테이블 할당
                    EmptyTableAndChairs = table;
                    occupiedTables.Add(table);
                    
                    currentState = NpcState.WALKING_TO_TABLE;
                    yield break;
                }
            }
            
            yield return new WaitForSeconds(1f); // 1초마다 체크
        }
        
        // 아직 차례가 아니면 계속 대기
        yield return new WaitForSeconds(0.5f);
    }
    
    private IEnumerator Coroutine_WalkToTable()
    {
        Debug.Log("NPC: Walking to table...");
        
        if (EmptyTableAndChairs != null)
        {
            // 빈 의자 찾기 (왼쪽 또는 오른쪽)
            Transform chairLeft = EmptyTableAndChairs.transform.Find("Chair Left");
            Transform chairRight = EmptyTableAndChairs.transform.Find("Chair Right");
            
            if (chairLeft != null && IsSeatEmpty(chairLeft))
            {
                sitPosition = chairLeft.Find("Sit Position");
                if (sitPosition != null)
                {
                    // 의자 앞으로 이동 (앉기 위한 위치)
                    Vector3 chairFrontPos = sitPosition.position + new Vector3(0, 0.5f, 0);
                    yield return MoveToPosition(chairFrontPos);
                }
            }
            else if (chairRight != null && IsSeatEmpty(chairRight))
            {
                sitPosition = chairRight.Find("Sit Position");
                if (sitPosition != null)
                {
                    // 의자 앞으로 이동 (앉기 위한 위치)
                    Vector3 chairFrontPos = sitPosition.position + new Vector3(0, 0.5f, 0);
                    yield return MoveToPosition(chairFrontPos);
                }
            }
            else
            {
                Debug.LogWarning("No empty seats at table!");
                // 테이블이 비어있지 않으면 다시 테이블 찾기
                occupiedTables.Remove(EmptyTableAndChairs);
                EmptyTableAndChairs = null;
                currentState = NpcState.FINDING_TABLE;
                yield break;
            }
            
            currentState = NpcState.SITTING;
        }
        else
        {
            currentState = NpcState.FINDING_TABLE;
        }
    }
    
    private IEnumerator Coroutine_Sit()
    {
        Debug.Log("NPC: Sitting...");
        
        if (sitPosition != null)
        {
            // 앉는 애니메이션 재생 (있다면)
            if (animator != null)
            {
                animator.SetInteger("MoveMode", (int)MoveMode.SITTING); // TODO: 로컬변수의 moveMode와 animator.MoveMode 를 동기화해야 함.
            }
            
            // 앉는 위치로 이동
            transform.position = sitPosition.position;
            
            // 3초 후 주문
            yield return new WaitForSeconds(3f);
            currentState = NpcState.WAITING_FOR_ORDER;
        }
        else
        {
            currentState = NpcState.FINDING_TABLE;
        }
    }
    
    private IEnumerator Coroutine_WaitForOrder()
    {
        Debug.Log("NPC: Waiting for order...");
        
        // 무작위 메뉴 선택
        int randomMenu = Random.Range(1, 4); // 1=WATER, 2=ESPRESSO, 3=AMERICANO
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
        
        // 주문 전달 대기
        while (!isOrderDelivered)
        {
            yield return new WaitForSeconds(0.5f);
        }
        
        currentState = NpcState.EATING;
    }
    
    private IEnumerator Coroutine_Eat()
    {
        Debug.Log("NPC: Eating/Drinking...");
        
        // 먹는/마시는 애니메이션 재생 (있다면)
        if (animator != null)
        {
            animator.SetTrigger("Eating");
        }
        
        // 5초 후 떠남
        yield return new WaitForSeconds(5f);
        currentState = NpcState.LEAVING;
    }
    
    private IEnumerator Coroutine_Leave()
    {
        Debug.Log("NPC: Leaving...");
        
        // 일어서는 애니메이션 재생 (있다면)
        if (animator != null)
        {
            animator.SetBool("IsSitting", false);
        }
        
        // 테이블 빈 상태로 만들기
        if (EmptyTableAndChairs != null)
        {
            occupiedTables.Remove(EmptyTableAndChairs);
        }
        
        // 스폰 위치로 돌아가기
        if (SpawnPosition != null)
        {
            yield return MoveToPosition(SpawnPosition.transform.position);
        }
        
        // NPC 제거
        Destroy(gameObject);
    }
    
    // 테이블이 비어있는지 확인하는 유틸리티 메서드
    private bool IsTableEmpty(GameObject table)
    {
        Transform chairLeft = table.transform.Find("Chair Left");
        Transform chairRight = table.transform.Find("Chair Right");
        
        return (chairLeft != null && IsSeatEmpty(chairLeft)) || 
               (chairRight != null && IsSeatEmpty(chairRight));
    }
    
    // 의자가 비어있는지 확인하는 유틸리티 메서드
    private bool IsSeatEmpty(Transform chair)
    {
        // Sit Position에 NPC가 있는지 확인
        Transform sitPos = chair.Find("Sit Position");
        if (sitPos != null)
        {
            // 주변 1유닛 반경 내에 다른 NPC가 있는지 검사
            Collider2D[] colliders = Physics2D.OverlapCircleAll(sitPos.position, 0.5f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.GetComponent<NpcMover>() != null && collider.gameObject != gameObject)
                {
                    return false; // 다른 NPC가 이미 앉아있음
                }
            }
            return true; // 비어있음
        }
        return false; // Sit Position이 없음
    }
    
    // 특정 위치로 이동하는 메서드 (x축과 y축 이동 분리)
    private IEnumerator MoveToPosition(Vector3 position)
    {
        moveMode = MoveMode.WALKING;
        targetPosition = position;
        
        // 먼저 x축 이동
        while (Mathf.Abs(transform.position.x - targetPosition.x) > 0.1f)
        {
            Vector3 direction = new Vector3(targetPosition.x - transform.position.x, 0, 0).normalized;
            MoveInDirection(direction);
            yield return null;
        }
        
        // 그 다음 y축 이동
        while (Mathf.Abs(transform.position.y - targetPosition.y) > 0.1f)
        {
            Vector3 direction = new Vector3(0, targetPosition.y - transform.position.y, 0).normalized;
            MoveInDirection(direction);
            yield return null;
        }
        
        moveMode = MoveMode.IDLE;
        moveDirection = MoveDirection.IDLE;
        UpdateAnimator();
    }
    
    // 방향에 따라 이동 및 애니메이션 처리
    private void MoveInDirection(Vector3 direction)
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
    private void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetInteger("MoveMode", (int)moveMode);
            animator.SetInteger("MoveDirection", (int)moveDirection);
        }
    }
    
    // 주문 배달 처리 (PlayerMover에서 호출)
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
    public bool IsSeatedAt(GameObject table)
    {
        return currentState == NpcState.SITTING || 
               currentState == NpcState.WAITING_FOR_ORDER || 
               currentState == NpcState.EATING && 
               EmptyTableAndChairs == table;
    }
}