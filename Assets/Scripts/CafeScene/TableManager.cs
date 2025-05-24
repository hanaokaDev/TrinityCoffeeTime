using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events; // for UnityAction

// 비어있는 Table의 점유현황을 관리하며 Alloc/Dealloc하는 싱글턴 클래스
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
                Debug.Log("MarkEmptyTable: returns " + i);
                return i;
            }
        }
        Debug.Log("MarkEmptyTable: No empty table found");
        return -1; // 모든 테이블이 사용 중인 경우
    }

    public TableAndChairs GetTable(int index)
    {
        if (index >= 0 && index < tables.Length)
        {
            return tables[index]; // 해당 인덱스의 테이블 반환
        }
        Debug.Log("Invalid table index: " + index);
        return null; // 유효하지 않은 인덱스인 경우 null 반환
    }
}
