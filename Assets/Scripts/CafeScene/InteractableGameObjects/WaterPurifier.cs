
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterPurifier : InteractableItem
{

    public override void OnClickInteractableGameObject()
    {
        Debug.Log("Water Machine Clicked");
        if(currentTime >= maxTime){
            bool isSuccess = chosenPlayer.AddItem(PlayerItemEnum.WATER); // 플레이어에게 물 아이템 추가
            if(isSuccess){
                progressBar.SetValue(0); // 정수기 리셋
            }
        }
        else{
            base.OnClickInteractableGameObject();
            return;
        }
    }

}
