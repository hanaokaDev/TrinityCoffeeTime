using UnityEngine;

public class TableAndChairs : MonoBehaviour
{
    // {{{ 설정할것
    public Sprite nextUseButtonSprite;
    public GameObject ServeUI; // 서빙 UI
    // }}}
    private ServeUIManager serveUIManager;
    private PlayerItem foodItem = PlayerItem.NONE; // 테이블에 놓일 음식 아이템
    private PlayerItem[] drinkItems = {PlayerItem.NONE, PlayerItem.NONE}; // 테이블에 놓일 음료 아이템

    private PlayerMover chosenPlayer; // 현재 선택된 플레이어
    public bool isTableOccupied = false;
    public NpcMover[] sitter = {null, null}; 

    [SerializeField]
    private Transform foodPosition;

    [SerializeField]
    private Transform[] drinkPositions = new Transform[2]; // 음식과 음료를 놓을 위치

    [SerializeField]
    private Transform[] chairSitPositions = new Transform[2]; // 의자에 앉을 위치

    void Start()
    {
        foodPosition = transform.Find("Round Table").Find("Food Position");
        drinkPositions[0] = transform.Find("Round Table").Find("Drink Position Left");
        drinkPositions[1] = transform.Find("Round Table").Find("Drink Position Right");
        chairSitPositions[0] = transform.Find("Chair Left").Find("Sit Position");
        chairSitPositions[1] = transform.Find("Chair Right").Find("Sit Position");
        serveUIManager = ServeUI.GetComponent<ServeUIManager>();
    }

    public Transform GetChairSitPosition(int index)
    {
        if(index < 0 || index >= chairSitPositions.Length)
        {
            Debug.LogError("Invalid chair index: " + index);
            return null;
        }
        return chairSitPositions[index];
    }
    public PlayerItem GetFood()
    {
        return foodItem;
    }
    public void SetFood(PlayerItem playerItem)
    {
        foodItem = playerItem;
        foodPosition.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetItemSprite(playerItem);
        foodPosition.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(.3f, .3f, 1); // 스프라이트 크기 조정

        if(sitter[0] != null)
        {
            sitter[0].DeliverOrder(playerItem);
        }
        if(sitter[1] != null)
        {
            sitter[1].DeliverOrder(playerItem);
        }
    }
    public PlayerItem GetDrink(int index)
    {
        if(index < 0 || index >= drinkItems.Length)
        {
            Debug.LogError("Invalid drink index: " + index);
            return PlayerItem.NONE;
        }
        return drinkItems[index];
    }
    public void SetDrink(PlayerItem playerItem, int index)
    {
        if(index < 0 || index >= drinkItems.Length)
        {
            Debug.LogError("Invalid drink index: " + index);
            return;
        }
        drinkItems[index] = playerItem;
        drinkPositions[index].GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetItemSprite(playerItem);
        Debug.Log("SetDrink: " + playerItem + " at index " + index);
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


    public void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerMover>();
        if(player != null && player.isOwned && isTableOccupied) {
            serveUIManager.tableAndChairs = this;
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
            serveUIManager.tableAndChairs = null;
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
