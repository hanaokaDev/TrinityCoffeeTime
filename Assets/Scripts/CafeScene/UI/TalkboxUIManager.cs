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

    public static TalkboxUIManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void StartDialogue(int dialogueId)
    {
        DialogueScript dialogueScript = Resources.Load<DialogueScript>($"DialogueScript_{dialogueId}");
        if (dialogueScript == null)
        {
            Debug.LogError($"StartDialogue: DialogueScript_{dialogueId} not found!");
            return;
        }

        talkBoxUI.Open(dialogueScript);
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