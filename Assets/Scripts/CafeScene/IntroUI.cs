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
        textList.Add("그날 인류는 떠올렸다.");
        textList.Add("그들에게 지배당하던 공포를.");
        textList.Add("새장 속에 갇혀 있던 굴욕을.");

        string text = textList[0];
        gameObject.SetActive(true);
        StartCoroutine(ShowText_Coroutine(textList));
    }

    // 글자가 시간에 따라 촤라락 바뀌도록 Coroutine으로 구현.
    private IEnumerator ShowText_Coroutine(List<string> texts)
    {
        targetText.text = "";
        for(int i = 0; i < texts.Count; i++)
        {
            string forwardText = "";
            string backText = texts[i];

            Debug.Log("ShowText_Coroutine Start");

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
        // yield return new WaitForSeconds(10f); // 10초동안 결과를 보여줌.
        Debug.Log("ShowText_Coroutine End");
        // onTextCompleted.Invoke(); // 이벤트 Invoke 해도 사용할곳이 없어서 비활성화함.
        Close();
    }
    public void Close()
    {
        gameObject.SetActive(false);   
    }
}
