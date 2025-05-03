using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ServeUI : MonoBehaviour
{
    [SerializeField]
    private int currentSelectIndex = 0;

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
        else{
            Debug.LogWarning("HandleCursorMovement: Else key pressed");
        }
    }

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