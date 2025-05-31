using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class DialogueCsvImporter
{
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

            int dialogueId = int.Parse(cols[0]);
            int stepId = int.Parse(cols[1]);
            string text = cols[2].Trim('"');

            DialogueStep step = new DialogueStep
            {
                stepId = stepId,
                text = text,
                choices = new List<DialogueChoice>()
            };

            // choice1
            if (!string.IsNullOrEmpty(cols[3]))
            {
                step.choices.Add(new DialogueChoice
                {
                    choiceText = cols[3],
                    nextStepId = string.IsNullOrEmpty(cols[4]) ? -1 : int.Parse(cols[4])
                });
            }
            // choice2
            if (!string.IsNullOrEmpty(cols[5]))
            {
                step.choices.Add(new DialogueChoice
                {
                    choiceText = cols[5],
                    nextStepId = string.IsNullOrEmpty(cols[6]) ? -1 : int.Parse(cols[6])
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