using UnityEngine;

public class TableAndChairs : MonoBehaviour
{
    public bool isTableOccupied = false;
    public bool[] isChairOccupied = {false, false};
    public Transform[] chairSitPositions = new Transform[2]; // 의자에 앉을 위치

    public Transform foodPosition;
    public PlayerItem foodItem; // 테이블에 놓일 음식 아이템
    public Transform[] drinkPositions = new Transform[2]; // 음식과 음료를 놓을 위치
    public PlayerItem[] drinkItems; // 테이블에 놓일 음료 아이템


}
