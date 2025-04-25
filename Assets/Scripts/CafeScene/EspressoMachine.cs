
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EspressoMachine : MonoBehaviour
{
    public enum State { IDLE, BUSY, DONE, DIRTY};
    public State currentState;

    public ProgressBar progressBar;

    int timeLeft = 0;
    public int brewTime = 5; // 초 단위로 설정

    Text myText;
    Slider mySlider;

    void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    void Start()
    {
        currentState = State.IDLE;
        progressBar.SetMaxValue(brewTime);
        progressBar.SetValue(0);
    }

    public void OnClickEspressoMachine()
    {
        Debug.Log("Espresso Machine Clicked");
        float currentValue = progressBar.GetValue();
        progressBar.SetValue(currentValue + 1);
    }
}


// public class HUD : MonoBehaviour
// {
    


    

//     void LateUpdate()
//     {
//         if(!GameManager.instance.isLive) return;
//         switch(type){
//             case InfoType.Exp:
//                 float curExp = GameManager.instance.exp;
//                 float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length-1)];
//                 mySlider.value = curExp / maxExp;
//                 break;
//             case InfoType.Level:
//                 myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
//                 break;
//             case InfoType.Kill:
//                 myText.text = string.Format("{0:F0}", GameManager.instance.kill);
//                 break;
//             case InfoType.Time:
//                 float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
//                 int min = Mathf.FloorToInt(remainTime / 60);
//                 int sec = Mathf.FloorToInt(remainTime % 60);
//                 myText.text = string.Format("{0:D2}:{1:D2}", min, sec); // D0, D1, D2: 자리수를 지정
//                 break;
//             case InfoType.Health:
//                 float maxHealth = GameManager.instance.maxHealth;
//                 float curHealth = GameManager.instance.health;
//                 mySlider.value = curHealth / maxHealth;
//                 break;
//             default:
//                 break;
//         }
//     }

// }
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
