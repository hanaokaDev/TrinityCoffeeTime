using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum CharacterIndex
{
    Yoshimi,
    Mika,
}

public class TalkboxUIManager : MonoBehaviour
{
    public TalkboxUI talkBoxUI;

    private bool isDialogueActive = false;



    public static TalkboxUIManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void StartDialogue(int dialogueId)
    {
        isDialogueActive = true;
        // GameManager.Instance.GameStateChangeHandler?.Invoke("", true); // 이벤트 호출
        
        GameManager.Instance.isPause = true; // 게임 일시정지
        DialogueScript dialogueScript = Resources.Load<DialogueScript>($"DialogueScript_{dialogueId}");
        if (dialogueScript == null)
        {
            Debug.LogError($"StartDialogue: DialogueScript_{dialogueId} not found!");
            return;
        }

        talkBoxUI.Open(dialogueScript);
    }

    public void CloseDialogue()
    {
        isDialogueActive = false;

        // 대화창 닫기
        talkBoxUI.Close();
    
        GameManager.Instance.isPause = false; // 게임 재개
        
        // 이벤트 발생
        // OnDialogueStateChanged?.Invoke(false);
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