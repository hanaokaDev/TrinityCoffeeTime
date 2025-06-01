using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class IntroUI : TalkboxUI
{

    private DialogueScript dialogueScript;

    public override void Open()
    {
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


        string text = textList[0];
        gameObject.SetActive(true);
        StartCoroutine(ShowTextList_Coroutine(textList));
    }

}
