
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EspressoMachine : InteractableItem
{

    public override void OnClickInteractableGameObject()
    {
        Debug.Log("Espresso Machine Clicked");
        if(currentTime >= maxTime){
            bool isSuccess = chosenPlayer.AddItem(PlayerItem.ESPRESSO); // 플레이어에게 물 아이템 추가
            if(isSuccess){
                progressBar.SetValue(0); // 기계 리셋
            }
        }
        else{
            base.OnClickInteractableGameObject();
            return;
        }
    }

}


public enum EspressoType
{
    NONE,
    ESPRESSO,
    AMERICANO,
    CARAMEL_MACCHIATO,
    CAPPUCCINO,
    LATTE,
    MOCHA,
    MACCHIATO,
    CARAMEL_LATTE,
    VANILLA_LATTE
}
