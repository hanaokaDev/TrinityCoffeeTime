using UnityEngine;

public class TableAndChairs : MonoBehaviour
{
    // {{{ 설정할것
    public Sprite nextUseButtonSprite;
    public GameObject ServeUI; // 서빙 UI
    // }}}

    private PlayerMover chosenPlayer; // 현재 선택된 플레이어
    public bool isTableOccupied = false;
    public NpcMover[] sitter = {null, null}; 
    public Transform[] chairSitPositions = new Transform[2]; // 의자에 앉을 위치

    public Transform foodPosition;
    public PlayerItem foodItem; // 테이블에 놓일 음식 아이템
    public Transform[] drinkPositions = new Transform[2]; // 음식과 음료를 놓을 위치
    public PlayerItem[] drinkItems; // 테이블에 놓일 음료 아이템

    void Start()
    {
        chairSitPositions[0] = transform.Find("Chair Left");
        chairSitPositions[1] = transform.Find("Chair Right");
    }

    public void PushNpc(NpcMover npc)
    {
        if (sitter[0] == null)
        {
            sitter[0] = npc;
        }
        else if (sitter[1] == null)
        {
            sitter[1] = npc;
        }
        else{
            Debug.Log("Table is full");
            return;
        }
    }

    public void FlushNpc()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerMover>();
        if(player != null && player.isOwned && isTableOccupied) {
            HudManager.Instance.SetUseButton(nextUseButtonSprite, OnClickUseButton);
        }
        else{
            Debug.LogWarning("OnTriggerEnter2D: Player is not owned or table is not occupied");
        }
        chosenPlayer = player;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerMover>();
        if(player != null && player.isOwned) {
            HudManager.Instance.UnsetUseButton();
        }
        else{
            Debug.LogWarning("OnTriggerExit2D: Player is not owned or table is not occupied");
        }
        chosenPlayer = null;
    } 

    public void OnClickUseButton()
    {
        ServeUI.SetActive(true); // 서빙 UI 활성화
    }

}
