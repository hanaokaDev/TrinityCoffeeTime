using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public const int MAXIMUM_TRAY_SIZE = 3;
    public static int MAXIMUM_ITEM_NUM = PlayerItem.GetNames(typeof(PlayerItem)).Length; // 최대 아이템 수

    protected Animator animator;
    public MoveDirection moveDirection;

    public Collider2D interactionCollider;
    
    public float moveSpeed = 5f; // 이동 속도

    public bool isOwned = true; // 플레이어 소유 여부. 멀티플레이 확장을 위해 추가함.

    public PlayerItem[] items = new PlayerItem[MAXIMUM_TRAY_SIZE]; // 플레이어가 소지할 수 있는 아이템 배열 (예: 물, 에스프레소, 아메리카노)


    public bool AddItem(PlayerItem item)
    {
        // 아이템을 인벤토리에 추가하는 로직
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == PlayerItem.NONE)
            {
                items[i] = item;
                HudManager.Instance.SetItemToTray(item, i); // 트레이에 아이템 추가
                Debug.Log("Added " + item + " to inventory at index " + i);
                return true;
            }
        }
        Debug.Log("Inventory is full! Cannot add " + item);
        return false;
    }
    public bool RemoveItem(PlayerItem item)
    {
        // 아이템을 인벤토리에서 삭제하는 로직
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item)
            {
                items[i] = PlayerItem.NONE;
                HudManager.Instance.SetItemToTray(PlayerItem.NONE, i); // 트레이에서 아이템 삭제
                Debug.Log("Removed " + item + " from inventory at index " + i);
                return true;
            }
        }
        Debug.Log(item + " not found in inventory!");
        return false;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        moveDirection = MoveDirection.IDLE;
        animator.SetInteger("MoveDirection", (int)moveDirection);

    }

    void FixedUpdate()
    {
        // 이동 처리
        Vector3 move = Vector3.zero;

        switch (moveDirection)
        {
            case MoveDirection.UP:
                move = Vector3.up * moveSpeed * Time.fixedDeltaTime;
                break;
            case MoveDirection.DOWN:
                move = Vector3.down * moveSpeed * Time.fixedDeltaTime;
                break;
            case MoveDirection.LEFT:
                move = Vector3.left * moveSpeed * Time.fixedDeltaTime;
                break;
            case MoveDirection.RIGHT:
                move = Vector3.right * moveSpeed * Time.fixedDeltaTime;
                break;
            default:
                break;
        }

        transform.Translate(move);
    }

    void Update()
    {
        if(InputManager.Instance.GetInputAlloc() != InputAlloc.PLAYER_MOVER) return;
        
        bool keyPressed = false;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            keyPressed = true;
            moveDirection = MoveDirection.UP;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            keyPressed = true;
            moveDirection = MoveDirection.DOWN;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            keyPressed = true;
            moveDirection = MoveDirection.LEFT;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            keyPressed = true;
            moveDirection = MoveDirection.RIGHT;
        }
        else{
            // 키가 눌리지 않았을 때 IDLE 상태로 설정
            keyPressed = false;
            moveDirection = MoveDirection.IDLE;
        }
        
        if(keyPressed){
            animator.SetBool("IsMoving", true);
            animator.SetInteger("MoveDirection", (int)moveDirection);
        }
        else{
            animator.SetBool("IsMoving", false);
        }
    }
}



public enum PlayerItem {
    NONE,
    WATER,
    ESPRESSO,
    AMERICANO
}


public enum MoveDirection
{
    IDLE,
    UP,
    DOWN,
    LEFT,
    RIGHT
}

