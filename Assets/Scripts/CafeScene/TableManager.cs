using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events; // for UnityAction

public class TableManager : MonoBehaviour
{
    public static TableManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public TableAndChairs[] tables; // 테이블과 의자 배열

    
    public int MarkEmptyTable()
    {
        for (int i = 0; i < tables.Length; i++)
        {
            if (!tables[i].isTableOccupied)
            {
                tables[i].isTableOccupied = true; // 테이블을 사용 중으로 표시
                return i;
            }
        }
        return -1; // 모든 테이블이 사용 중인 경우
    }
}
