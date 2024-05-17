using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "new_Enemy", menuName = "Data/Enemy")]
public class EnemyData : ScriptableObjectBase
{
    public int BaseHealth;
    public Sprite Sprite;
    public Stats Stats;
    [SerializeField, SerializeReference]
    public List<Effect> Attacks;
    public Enums.EnemyType Type;

    public EnemyData Clone()
    {
        var clone = CreateInstance<EnemyData>();
        clone.Id = Id;
        clone.Name = Name;
        clone.Description = Description;
        clone.BaseHealth = BaseHealth;
        clone.Sprite = Sprite;
        clone.Stats = Stats;
        clone.Attacks = Attacks;
        clone.Type = Type;

        return clone;
    }
#if UNITY_EDITOR
    [PropertyOrder(-1)]
    [Button("Generate Id", ButtonSizes.Medium, 0)]
    protected override void GenerateUniqueId()
    {
        string prefix = "E";
        int id = 1;

        string[] existingAssetPaths = AssetDatabase.FindAssets("t:EnemyData", new[] { "Assets/Data" });
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
