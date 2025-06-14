using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using DG.Tweening; // DOTween을 사용하기 위해 추가


public class TalkboxUI : MonoBehaviour
{
    [SerializeField] private float animationDuration = 10f;
    [SerializeField] private Ease easeType = Ease.OutBack;


    public RectTransform boxRect;
    [SerializeField]
    protected Text targetText;
    // Open 메서드 수정 
    public void Open(DialogueScript dialogueScript)
    {
        if (dialogueScript == null || dialogueScript.steps.Count == 0)
        {
            Debug.LogError("DialogueScript is null or has no steps.");
            return;
        }

        // 먼저 활성화
        gameObject.SetActive(true);

        // 초기 상태 설정 (작은 크기)
        boxRect.localScale = new Vector3(0.5f, 0.3f, 1f);

        // 애니메이션으로 확대
        boxRect.DOScale(Vector3.one, animationDuration)
            .SetEase(easeType)
            .OnComplete(() => StartDialogue(dialogueScript));
    }
    // 닫기 애니메이션 추가
    public void Close()
    {
        // 애니메이션으로 축소 후 비활성화
        boxRect.DOScale(new Vector3(0.5f, 0.3f, 1f), animationDuration * 0.7f)
            .SetEase(Ease.InBack)
            .OnComplete(() => gameObject.SetActive(false));
    }

    public void StartDialogue(DialogueScript dialogueScript)
    {
        if (dialogueScript == null || dialogueScript.steps.Count == 0)
        {
            Debug.LogError("DialogueScript is null or has no steps.");
            return;
        }

        List<string> textList = new List<string>();
        foreach (var step in dialogueScript.steps)
        {
            textList.Add(step.text);
        }

        StartCoroutine(ShowTextList_Coroutine(textList));
    }


    protected IEnumerator ShowText_Coroutine(string text)
    {
        targetText.text = "";
        string forwardText = "";
        string backText = text;

        Debug.Log("ShowText_Coroutine Start");

        // 적당한 딜레이를 주면서 글자를 순차적으로 출력.
        while (backText.Length != 0)
        {
            forwardText += backText[0];
            backText = backText.Remove(0, 1);
            targetText.text = string.Format("<color=#FFFFFF>{0}</color><color=#000000>{1}</color>", forwardText, backText);
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("ShowText_Coroutine End");
        // onTextCompleted.Invoke(); // 이벤트 Invoke 해도 사용할곳이 없어서 비활성화함.
    }

    // 글자가 시간에 따라 촤라락 바뀌도록 Coroutine으로 구현.
    protected IEnumerator ShowTextList_Coroutine(List<string> texts)
    {
        targetText.text = "";
        for (int i = 0; i < texts.Count; i++)
        {
            string text = texts[i];
            Debug.Log($"ShowTextList_Coroutine Start: {text}");
            yield return StartCoroutine(ShowText_Coroutine(text));
            yield return new WaitForSeconds(0.5f); // 다음 텍스트로 넘어가기 전에 잠시 대기
        }
        Debug.Log("ShowTextList_Coroutine End");
        // onTextCompleted.Invoke(); // 이벤트 Invoke 해도 사용할곳이 없어서 비활성화함.
        Close();
    }

    public void OnSkipButtonClick()
    {
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.ButtonSelect); // 버튼 클릭 사운드 재생
        Debug.Log("Skip Button Clicked"); // 스킵 버튼 클릭 시 로그 출력
        Close(); // 스킵 버튼 클릭 시 대화창 닫기
    }
}
