using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System; // for UnityAction

public class MenuRowUI : MonoBehaviour
{
    public const int MAXIMUM_MATERIAL_NUM = 3;

    public PlayerMover player;

    public Button confirmButton;

    public PlayerItem[] selectedItems = new PlayerItem[MAXIMUM_MATERIAL_NUM]; // 필요한 재료
    public PlayerItem resultItem;

    private bool IsCookReady()
    {
        // int[] isReady = new int[]; // 재료 준비 여부
        // for (int i = 0; i < MAXIMUM_MATERIAL_NUM; i++){
        //     isReady[i] = 0;
        // }



        for (int i = 0; i < MAXIMUM_MATERIAL_NUM; i++)
        {
            if (selectedItems[i] == PlayerItem.NONE) return false;
        }
        return true;
    }

    public void OnClickConfirmButton()
    {

    }
    
}
