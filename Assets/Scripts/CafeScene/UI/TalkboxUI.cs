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

public class TalkboxUI : MonoBehaviour
{
    public static TalkboxUI Instance;
    private void Awake()
    {
        Instance = this;
    }

    public Image[] characterImages; // 캐릭터 이미지 배열 <- Inspector에서 설정

    [SerializeField]
    private Text targetText;


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

    public class DialogueManager : MonoBehaviour
    {
        public DialogueScript dialogueScript;

        public void StartDialogue(int dialogueId)
        {
            dialogueScript = Resources.Load<DialogueScript>($"DialogueScript_{dialogueId}");
            if (dialogueScript == null)
            {
                Debug.LogError("DialogueScript not found!");
                return;
            }
            ShowStep(1);
        }

        public void ShowStep(int stepId)
        {
            var step = dialogueScript.steps.Find(s => s.stepId == stepId);
            if (step == null) return;

            Debug.Log(step.text);
            for (int i = 0; i < step.choices.Count; i++)
            {
                Debug.Log($"{i + 1}. {step.choices[i].choiceText} (다음: {step.choices[i].nextStepId})");
            }
            // 실제 UI에 텍스트와 선택지를 표시하는 코드를 여기에 작성
        }
    }

    public virtual void Open()
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
    protected IEnumerator ShowTextList_Coroutine(List<string> texts)
    {
        targetText.text = "";
        for (int i = 0; i < texts.Count; i++)
        {
            string forwardText = "";
            string backText = texts[i];

            Debug.Log("ShowTextList_Coroutine Start");

            // 적당한 딜레이를 주면서 글자를 순차적으로 출력.
            while (backText.Length != 0)
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
