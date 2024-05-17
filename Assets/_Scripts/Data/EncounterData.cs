using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "Data/Encounter")]
public class EncounterData : ScriptableObjectBase
{
    public List<EnemyData> enemies; // List of enemies in the encounter 
    public Enums.NodeType Type;
    // Add any other properties for special conditions or rewards here

    // Method to clone the Encounter object
    public EncounterData Clone()
    {
        EncounterData newEncounter = CreateInstance<EncounterData>();
        newEncounter.enemies = new List<EnemyData>();
        foreach (EnemyData enemy in enemies)
        {
            newEncounter.enemies.Add(enemy.Clone());
        }
        newEncounter.Type = Type;
        // Copy other properties here if needed
        return newEncounter;
    }
#if UNITY_EDITOR
    [PropertyOrder(-1)]
    [Button("Generate Id", ButtonSizes.Medium, 0)]
    protected override void GenerateUniqueId()
    {
        string prefix = "B";
        int id = 1;

        string[] existingAssetPaths = AssetDatabase.FindAssets("t:EncounterData", new[] { "Assets/Data" });
        foreach (string assetPath in existingAssetPaths)
        {
            string assetPathInProject = AssetDatabase.GUIDToAssetPath(assetPath);
            ScriptableObjectBase obj = AssetDatabase.LoadAssetAtPath<ScriptableObjectBase>(assetPathInProject);
            if (obj != null && obj.Id.StartsWith(prefix))
            {
                int existingId;
                if (int.TryParse(obj.Id.Substring(1), out existingId))
                {
                    id = Mathf.Max(id, existingId + 1);
                }
            }
        }
        Id = prefix + id.ToString("D4");
    }
#endif
}
