using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DialogueStep
{
    public int stepId;
    public string text;
    public List<DialogueChoice> choices = new List<DialogueChoice>();
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public int nextStepId;
}

[CreateAssetMenu(fileName = "DialogueScript", menuName = "ScriptableObjects/DialogueScript", order = 1)]
public class DialogueScript : ScriptableObject
{
    public int dialogueId;
    public List<DialogueStep> steps = new List<DialogueStep>();
}