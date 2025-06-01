using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class DialogueCsvImporter
{
    private const int DIALOGUE_ID = 0;
    private const int STEP_ID = 1;
    private const int TEXT = 2;
    private const int CHOICE1_TEXT = 3;
    private const int CHOICE1_NEXT_STEP_ID = 4;
    private const int CHOICE2_TEXT = 5;
    private const int CHOICE2_NEXT_STEP_ID = 6;

    [MenuItem("Tools/Import Dialogue CSV")]
    public static void ImportDialogueCsv()
    {
        string path = EditorUtility.OpenFilePanel("Select Dialogue CSV", Application.dataPath, "csv");
        if (string.IsNullOrEmpty(path)) return;

        var lines = File.ReadAllLines(path);
        Dictionary<int, DialogueScript> dialogueDict = new Dictionary<int, DialogueScript>();

        for (int i = 1; i < lines.Length; i++) // 0번은 헤더
        {
            var cols = lines[i].Split(',');

            int dialogueId = int.Parse(cols[DIALOGUE_ID]);
            int stepId = int.Parse(cols[STEP_ID]);
            string text = cols[TEXT].Trim('"');

            DialogueStep step = new DialogueStep
            {
                stepId = stepId,
                text = text,
                choices = new List<DialogueChoice>()
            };

            // choice1
            if (!string.IsNullOrEmpty(cols[CHOICE1_TEXT]))
            {
                step.choices.Add(new DialogueChoice
                {
                    choiceText = cols[CHOICE1_TEXT],
                    nextStepId = string.IsNullOrEmpty(cols[CHOICE1_NEXT_STEP_ID]) ? -1 : int.Parse(cols[CHOICE1_NEXT_STEP_ID])
                });
            }
            // choice2
            if (!string.IsNullOrEmpty(cols[CHOICE2_TEXT]))
            {
                step.choices.Add(new DialogueChoice
                {
                    choiceText = cols[CHOICE2_TEXT],
                    nextStepId = string.IsNullOrEmpty(cols[CHOICE2_NEXT_STEP_ID]) ? -1 : int.Parse(cols[CHOICE2_NEXT_STEP_ID])
                });
            }

            // ScriptableObject 생성 및 데이터 추가
            DialogueScript script;
            if (!dialogueDict.TryGetValue(dialogueId, out script))
            {
                script = ScriptableObject.CreateInstance<DialogueScript>();
                script.dialogueId = dialogueId;
                script.steps = new List<DialogueStep>();
                dialogueDict.Add(dialogueId, script);
            }
            script.steps.Add(step);
        }

        // ScriptableObject 저장
        foreach (var pair in dialogueDict)
        {
            string assetPath = $"Assets/Resources/DialogueScript_{pair.Key}.asset";
            AssetDatabase.CreateAsset(pair.Value, assetPath);
            EditorUtility.SetDirty(pair.Value);
        }
        AssetDatabase.SaveAssets();
        Debug.Log("Dialogue CSV import complete!");
    }
}