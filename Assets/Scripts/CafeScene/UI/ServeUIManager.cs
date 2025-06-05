using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ServeUIManager : MonoBehaviour
{
    public static ServeUIManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    [SerializeField]
    private int currentSelectIndex = 0;
    // {{{ Must Set
    public PlayerMover player; // 플레이어
    // }}}

    public TableAndChairs tableAndChairs; // 테이블과 의자
    public GameObject[] selectIcons; // 현재 선택된 Tray 아이템을 표시할 UI 아이콘들

    void OnEnable()
    {
        currentSelectIndex = 0;
        InputManager.Instance.SetInputAlloc(InputAlloc.SERVE_UI);
    }
    void OnDisable()
    {
        // 플레이어 입력 활성화
        InputManager.Instance.SetInputAlloc(InputAlloc.PLAYER_MOVER);
    }

    public void OnClickExitButton()
    {
        // 서빙 UI 비활성화
        gameObject.SetActive(false);
    }

    void Update()
    {
        if(InputManager.Instance.GetInputAlloc() != InputAlloc.SERVE_UI) return;
        HandleCursorMovement();
    }

    

    // 상하좌우 키 입력 처리
    private void HandleCursorMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            MoveToSelectable(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            MoveToSelectable(Vector3.right);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            ServeFood();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            ReclaimFood();
        }
    }
    private void ServeFood() // 현재 선택된 아이템을 테이블에게 전달
    {
        if (!selectIcons[currentSelectIndex].activeSelf) {
            Debug.LogWarning("Selected item is not in active cursor!");
            return;
        }

        PlayerItemEnum selectedItem = player.items[currentSelectIndex];
        if (selectedItem != PlayerItemEnum.NONE)
        {
            // 서빙 아이템을 플레이어에게 전달
            tableAndChairs.SetFood(selectedItem);
            player.RemoveItem(selectedItem);
            Debug.Log("Served item: " + selectedItem);
        }
    }
    private void ReclaimFood()
    {
        int emptyTrayItemIndex = -1;
        for(int trayItemIndex=0; trayItemIndex<PlayerMover.MAXIMUM_TRAY_SIZE; trayItemIndex++) {
            if(player.items[trayItemIndex] == PlayerItemEnum.NONE) {
                emptyTrayItemIndex = trayItemIndex;
                break;
            }
        }
        if(emptyTrayItemIndex == -1){
            Debug.LogWarning("No empty tray item index available!");
            return;
        }
        if(tableAndChairs.GetFood() == PlayerItemEnum.NONE) {
            Debug.LogWarning("No food item on table to reclaim!");
            return;
        }
        player.AddItem(tableAndChairs.GetFood()); // 플레이어에게 아이템 추가
        tableAndChairs.SetFood(PlayerItemEnum.NONE); // 테이블의 음식 아이템을 NONE으로 설정
    }

    // private void ServeDrink(int index)
    // {
    //     // 현재 선택된 아이템을 테이블에게 전달
    //     if (selectIcons[currentSelectIndex].activeSelf) {
    //         Debug.LogWarning("Selected item is not in active cursor!");
    //         return;
    //     }

    //     PlayerItemEnum selectedItem = player.items[currentSelectIndex];
    //     if (selectedItem != PlayerItemEnum.NONE)
    //     {
    //         // 서빙 아이템을 플레이어에게 전달
    //         tableAndChairs.SetDrink(selectedItem, index);
    //         player.RemoveItem(selectedItem); // TODO: 동일한 상품이 Tray에 있을 경우, Cursor가 Set된 상품이 제거되게끔, RemoveItemByIndex를 만들어야 함.
    //         Debug.Log("Served item: " + selectedItem);
    //     }
    // }
    // private void ReclaimDrink(int index)
    // {
    //     int emptyTrayItemIndex = -1;
    //     for(int trayItemIndex=0; trayItemIndex<PlayerMover.MAXIMUM_TRAY_SIZE; trayItemIndex++) {
    //         if(player.items[trayItemIndex] == PlayerItemEnum.NONE) {
    //             emptyTrayItemIndex = trayItemIndex;
    //             break;
    //         }
    //     }
    //     if(emptyTrayItemIndex == -1){
    //         Debug.LogWarning("No empty tray item index available!");
    //         return;
    //     }
    //     if(tableAndChairs.GetDrink(index) == PlayerItemEnum.NONE) {
    //         Debug.LogWarning("No drink item on table to reclaim!");
    //         return;
    //     }
    //     player.AddItem(tableAndChairs.GetDrink(index)); // 플레이어에게 아이템 추가
    //     tableAndChairs.SetDrink(PlayerItemEnum.NONE, index); // 테이블의 음식 아이템을 NONE으로 설정
    // }


    // Selectable 간 이동
    private void MoveToSelectable(Vector3 direction)
    {
        int nextSelectIndex = currentSelectIndex;
        if(direction == Vector3.left)
        {
            if(currentSelectIndex > 0)
            {
                nextSelectIndex = currentSelectIndex - 1;
            }
            else // wrap around
            {
                nextSelectIndex = selectIcons.Length - 1;
            }
        }
        else if(direction == Vector3.right)
        {
            if(currentSelectIndex < selectIcons.Length - 1)
            {
                nextSelectIndex = currentSelectIndex + 1;
            }
            else // wrap around
            {
                nextSelectIndex = 0;
            }
        }

        if(nextSelectIndex != currentSelectIndex)
        {
            // 현재 선택된 아이콘 비활성화
            selectIcons[currentSelectIndex].SetActive(false);
            // 다음 선택된 아이콘 활성화
            selectIcons[nextSelectIndex].SetActive(true);

            // 현재 선택 인덱스 업데이트
            currentSelectIndex = nextSelectIndex;
            // 선택된 아이템에 대한 추가 작업 수행 (예: 아이템 사용)
            Debug.Log("Changed selected item index: " + currentSelectIndex);
        }
    }

}