using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class IntroUI : MonoBehaviour
{

    [SerializeField]
    private Text targetText;

    // public UnityEvent onTextCompleted = new UnityEvent();

    
    public void Open()
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

    // 글자가 시간에 따라 촤라락 바뀌도록 Coroutine으로 구현.
    private IEnumerator ShowTextList_Coroutine(List<string> texts)
    {
        targetText.text = "";
        for(int i = 0; i < texts.Count; i++)
        {
            string forwardText = "";
            string backText = texts[i];

            Debug.Log("ShowTextList_Coroutine Start");

            // 적당한 딜레이를 주면서 글자를 순차적으로 출력.
            while(backText.Length != 0)
            {
                forwardText += backText[0];
                backText = backText.Remove(0, 1);
                targetText.text = string.Format("<color=#FFFFFF>{0}</color><color=#000000>{1}</color>", forwardText, backText);
                yield return new WaitForSeconds(0.1f);
            }
            
            yield return new WaitForSeconds(1f); // 다음 문장으로 넘어가기 전 1초 대기.
        }
        Debug.Log("ShowTextList_Coroutine End");
        // onTextCompleted.Invoke(); // 이벤트 Invoke 해도 사용할곳이 없어서 비활성화함.
        Close();
    }
    public void Close()
    {
        gameObject.SetActive(false);   
    }

    public void OnSkipButtonClick()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ButtonSelect); // 버튼 클릭 사운드 재생
        Debug.Log("Skip Button Clicked"); // 스킵 버튼 클릭 시 로그 출력
        Close(); // UI 닫기
    }
}
