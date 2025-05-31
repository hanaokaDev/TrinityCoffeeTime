using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class IntroUI : TalkboxUI
{

    [SerializeField]
    private Text targetText;

    public override void Open()
    {
        List<string> textList = new List<string>();
        textList.Add("어 아이리?");
        textList.Add("마침 잘 왔어. 지금 한참 손이 모자라거든.");
        textList.Add("이제 막 오픈한 카페라서 손님이 많아.");
        textList.Add("이런 날에 한탕 벌지 못하면 정말 큰일 나지.");
        textList.Add("아무튼, 오늘은 특별히 손님이 많아서");
        textList.Add("너한테 일을 부탁할게.");

        string text = textList[0];
        gameObject.SetActive(true);
        StartCoroutine(ShowTextList_Coroutine(textList));
    }

}
