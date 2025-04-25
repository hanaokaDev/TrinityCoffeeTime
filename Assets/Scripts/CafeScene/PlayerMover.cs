using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    protected Animator animator;
    public MoveDirection moveDirection;
    public float moveSpeed = 5f; // 이동 속도

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


public enum MoveDirection
{
    IDLE,
    UP,
    DOWN,
    LEFT,
    RIGHT
}

