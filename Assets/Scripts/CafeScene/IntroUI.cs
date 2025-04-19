using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class IntroUI : MonoBehaviour
{

    [SerializeField]
    private Text targetText;

    public UnityEvent onTextCompleted = new UnityEvent();

    
    public void Open()
    {
        string text = "이것은 테스트용 UI입니다. 하나둘삼넷오 여섯일곱여덟아홉열";
        gameObject.SetActive(true);
        StartCoroutine(ShowText_Coroutine(text));
    }

    // 글자가 시간에 따라 촤라락 바뀌도록 Coroutine으로 구현.
    private IEnumerator ShowText_Coroutine(string text)
    {
        targetText.text = "";
        string forwardText = "";
        string backText = text;

        Debug.Log("ShowText_Coroutine Start");

        // 적당한 딜레이를 주면서 글자를 순차적으로 출력.
        while(backText.Length != 0)
        {
            forwardText += backText[0];
            backText = backText.Remove(0, 1);
            targetText.text = string.Format("<color=#FFFFFF>{0}</color><color=#000000>{1}</color>", forwardText, backText);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(10f); // 10초동안 결과를 보여줌.
        Close();
        Debug.Log("ShowText_Coroutine End");
        onTextCompleted.Invoke();
    }
    public void Close()
    {
        gameObject.SetActive(false);   
    }
}
