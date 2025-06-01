using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class IntroUI : MonoBehaviour
{

    private DialogueScript dialogueScript;

    public static IntroUI Instance;
    private void Awake()
    {
        Instance = this;
    }

    public Image[] characterImages; // 캐릭터 이미지 배열 <- Inspector에서 설정

    [SerializeField]
    protected Text targetText;


    // public UnityEvent onTextCompleted = new UnityEvent();

    // TODO
    public virtual void RightCharacterJump()
    {
        // characterImages[(int)CharacterIndex.Yoshimi].transform.localPosition += new Vector3(0, 50, 0);
    }
    public virtual void LeftCharacterJump()
    {
        // characterImages[(int)CharacterIndex.Mika].transform.localPosition += new Vector3(0, 50, 0);
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);

        dialogueScript = Resources.Load<DialogueScript>("DialogueScript_100");
        if (dialogueScript == null)
        {
            Debug.LogError("DialogueScript not found!");
            return;
        }

        List<string> textList = new List<string>();
        foreach (var step in dialogueScript.steps)
        {
            textList.Add(step.text);
        }

        if (textList.Count == 0)
        {
            Debug.LogWarning("DialogueScript_100에 대사가 없습니다.");
            return;
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
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnSkipButtonClick()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ButtonSelect); // 버튼 클릭 사운드 재생
        Debug.Log("Skip Button Clicked"); // 스킵 버튼 클릭 시 로그 출력
        Close(); // UI 닫기
    }
}



// public class DialogueManager : MonoBehaviour
// {
//     public DialogueScript dialogueScript;

//     public void StartDialogue(int dialogueId)
//     {
//         dialogueScript = Resources.Load<DialogueScript>($"DialogueScript_{dialogueId}");
//         if (dialogueScript == null)
//         {
//             Debug.LogError("DialogueScript not found!");
//             return;
//         }
//         ShowStep(1);
//     }

//     public void ShowStep(int stepId)
//     {
//         var step = dialogueScript.steps.Find(s => s.stepId == stepId);
//         if (step == null) return;

//         Debug.Log(step.text);
//         for (int i = 0; i < step.choices.Count; i++)
//         {
//             Debug.Log($"{i + 1}. {step.choices[i].choiceText} (다음: {step.choices[i].nextStepId})");
//         }
//         // 실제 UI에 텍스트와 선택지를 표시하는 코드를 여기에 작성
//     }
// }