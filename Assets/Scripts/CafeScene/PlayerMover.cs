using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    protected Animator animator;
    public MoveDirection moveDirection;

    void Start()
    {
        animator = GetComponent<Animator>();
        moveDirection = MoveDirection.IDLE;
        animator.SetInteger("MoveDirection", (int)moveDirection);
    }

    void Update()
    {
        bool keyPressed = false;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            keyPressed = true;
            moveDirection = MoveDirection.UP;
            Debug.Log("Moving UP");
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            keyPressed = true;
            moveDirection = MoveDirection.DOWN;
            Debug.Log("Moving DOWN");
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            keyPressed = true;
            moveDirection = MoveDirection.LEFT;
            Debug.Log("Moving LEFT");
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            keyPressed = true;
            moveDirection = MoveDirection.RIGHT;
            Debug.Log("Moving RIGHT");
        }
        else{
            // 키가 눌리지 않았을 때 IDLE 상태로 설정
            keyPressed = true;
            moveDirection = MoveDirection.IDLE;
            Debug.Log("IDLE");
        }
        
        if(keyPressed){
            animator.SetInteger("MoveDirection", (int)moveDirection);
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

