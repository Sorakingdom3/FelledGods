using Sirenix.OdinInspector;
using System;
using UnityEditor;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "new_Item", menuName = "Data/Item")]
public class ItemData : ScriptableObjectBase
{
    public Enums.RarityType Rarity;

    public Enums.AllowedTarget Target;

    [SerializeField, SerializeReference]
    public Effect Effect;
#if UNITY_EDITOR
    [PropertyOrder(-1)]
    [Button("Generate Id", ButtonSizes.Medium, 0)]
    protected override void GenerateUniqueId()
    {
        string prefix = "I";
        int id = 1;

        string[] existingAssetPaths = AssetDatabase.FindAssets("t:Item", new[] { "Assets/Data" });
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
