using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;

public class CsvDialogueRow
{
    public int dialogueId { get; set; }
    public int stepId { get; set; }
    public string text { get; set; }
    public string choice1Text { get; set; }
    public int? choice1NextStepId { get; set; }
    public string choice2Text { get; set; }
    public int? choice2NextStepId { get; set; }
}

public class CsvDialogueRowMap : ClassMap<CsvDialogueRow>
{
    public CsvDialogueRowMap()
    {
        Map(m => m.dialogueId).Name("dialogueId");
        Map(m => m.stepId).Name("stepId");
        Map(m => m.text).Name("text");
        Map(m => m.choice1Text).Name("choice1Text").Optional();
        Map(m => m.choice1NextStepId).Name("choice1NextStepId").Optional();
        Map(m => m.choice2Text).Name("choice2Text").Optional();
        Map(m => m.choice2NextStepId).Name("choice2NextStepId").Optional();
    }
}

public class DialogueCsvImporter
{
    [MenuItem("myTools/Import Dialogue CSV")]
    public static void ImportDialogueCsv()
    {
        string path = EditorUtility.OpenFilePanel("Select Dialogue CSV", Application.dataPath, "csv");
        if (string.IsNullOrEmpty(path)) return;

        Dictionary<int, DialogueScript> dialogueDict = new Dictionary<int, DialogueScript>();


        // var lines = File.ReadAllLines(path);
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            // TrimOptions = TrimOptions.Trim
            // 헤더 자동 매핑
            csv.Context.RegisterClassMap<CsvDialogueRowMap>(); // ClassMap 등록 (최신 버전)
            var records = csv.GetRecords<CsvDialogueRow>();

            foreach (var row in records)
            {
                DialogueStep step = new DialogueStep
                {
                    stepId = row.stepId,
                    text = row.text,
                    choices = new List<DialogueChoice>()
                };

                // choice1
                if (!string.IsNullOrEmpty(row.choice1Text))
                {
                    step.choices.Add(new DialogueChoice
                    {
                        choiceText = row.choice1Text,
                        nextStepId = row.choice1NextStepId != null ? row.choice1NextStepId.Value : 0
                    });
                }
                // choice2
                if (!string.IsNullOrEmpty(row.choice2Text))
                {
                    step.choices.Add(new DialogueChoice
                    {
                        choiceText = row.choice2Text,
                        nextStepId = row.choice2NextStepId != null ? row.choice2NextStepId.Value : 0    
                    });
                }

                DialogueScript script;
                if (!dialogueDict.TryGetValue(row.dialogueId, out script))
                {
                    script = ScriptableObject.CreateInstance<DialogueScript>();
                    script.dialogueId = row.dialogueId;
                    script.steps = new List<DialogueStep>();
                    dialogueDict.Add(row.dialogueId, script);
                }
                script.steps.Add(step);
            }
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