using System.Collections;
using UnityEngine;


public class MikaMover : NpcMover
{
    public TalkboxUIManager talkBoxUIManager;

    protected override IEnumerator Coroutine_Eat()
    {
        Debug.Log("NPC: Eating/Drinking...");

        SpeakBubbleActive("That's what I wanted!", 3f);
        moveMode = MoveMode.EATING;
        if (animator != null)
        {
            UpdateAnimator();
            print("Eating Animation Triggered");
        }
        else
        {
            Debug.LogWarning("Animator component not found on NPC!");
        }

        // 5초 후 떠남
        yield return new WaitForSeconds(5f);
        currentState = NpcState.LEAVING;
    }

    
    protected override IEnumerator Coroutine_Leave()
    {
        TalkboxUIManager.Instance.StartDialogue(101);
        yield return base.Coroutine_Leave();
    }

}