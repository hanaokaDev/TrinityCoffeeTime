using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System; // for UnityAction

/*
 * 요리가능여부를 판단하여, 플레이어에게 요리결과물을 주거나 아니면 재료부족으로 실패시킴.
*/
public class MenuRow : MonoBehaviour
{
    // {{{ To set
    public PlayerMover player;

    public Button confirmButton;
    
    // }}}
    public const int MAXIMUM_MATERIAL_NUM = 3;

    public PlayerItemEnum[] selectedItems = new PlayerItemEnum[MAXIMUM_MATERIAL_NUM]; // 필요한 재료
    public PlayerItemEnum resultItem;

    private bool IsCookReady()
    {
        Boolean[] isTraySelected = new Boolean[PlayerMover.MAXIMUM_TRAY_SIZE];
        for (int currentMaterialIndex = 0; currentMaterialIndex < PlayerMover.MAXIMUM_TRAY_SIZE; currentMaterialIndex++){
            isTraySelected[currentMaterialIndex] = false;
        }

        for(int currentMaterialIndex=0; currentMaterialIndex<MAXIMUM_MATERIAL_NUM; currentMaterialIndex++){
            if(selectedItems[currentMaterialIndex] == PlayerItemEnum.NONE) continue;

            // 해당 재료를 tray가 가지고있는지 체크
            for(int trayItemIndex=0; trayItemIndex<PlayerMover.MAXIMUM_TRAY_SIZE; trayItemIndex++){
                if(isTraySelected[trayItemIndex] == true) continue; // 이미 선택된 재료는 스킵

                if(player.playerItems[trayItemIndex].data.itemType == selectedItems[currentMaterialIndex]){
                    isTraySelected[trayItemIndex] = true;
                    break;
                }

                if(trayItemIndex == PlayerMover.MAXIMUM_TRAY_SIZE - 1){
                    Debug.Log("Tray does not have " + selectedItems[currentMaterialIndex] + "!");
                    return false; // 마지막 아이템까지 체크했는데 없으면 false
                }
            }
        }
        Debug.Log("Tray has all needed items!");
        return true; // 모든 재료가 Tray에 존재하면 true.
    }

    private void RemoveMaterialsFromTray(){
        for(int currentMaterialIndex=0; currentMaterialIndex<MAXIMUM_MATERIAL_NUM; currentMaterialIndex++){
            if(selectedItems[currentMaterialIndex] == PlayerItemEnum.NONE) continue;

            // 해당 재료를 tray가 가지고있는지 체크
            for(int trayItemIndex=0; trayItemIndex<PlayerMover.MAXIMUM_TRAY_SIZE; trayItemIndex++){
                if(player.playerItems[trayItemIndex].data.itemType == selectedItems[currentMaterialIndex]){
                    player.RemoveItem(selectedItems[currentMaterialIndex]);
                    break;
                }
            }
        }
    }
    private void AddResultToTray(){
        bool isSuccess = player.AddItem(new PlayerItemData(resultItem, new PlayerItemEnum[] { })); // 플레이어에게 아이템 추가
        if(isSuccess){
            Debug.Log("Added " + resultItem + " to inventory!");
        }
    }

    public void OnClickConfirmButton()
    {
        if(!IsCookReady()){
            Debug.Log("Not enough materials to cook!");
            return;
        }
        RemoveMaterialsFromTray();
        AddResultToTray();
        Debug.Log("Cooking successful! Added " + resultItem + " to inventory!");
    }
    
}
