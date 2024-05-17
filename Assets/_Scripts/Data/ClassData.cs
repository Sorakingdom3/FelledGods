using Sirenix.OdinInspector;
using System;
using UnityEditor;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "new_Class", menuName = "Data/Class")]
public class ClassData : ScriptableObjectBase
{
    public int BaseHealth;
    public int BaseEnergy;
    public int BaseHandSize;
    public Stats BaseStats;
    public Sprite Sprite;
    public CardData[] BaseDeck;
    public CardData[] ClassCards;

    [PropertyOrder(-1)]
    [Button("Generate Id", ButtonSizes.Medium, 0)]
    protected override void GenerateUniqueId()
    {
        string prefix = "P";
        int id = 1;

        string[] existingAssetPaths = AssetDatabase.FindAssets("t:ClassDatas", new[] { "Assets/Data" });
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

}