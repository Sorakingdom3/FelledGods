using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "new_Card", menuName = "Data/Card")]
public class CardData : ScriptableObjectBase
{

    public int Cost;

    public Sprite CardSprite;
    public Sprite BorderSprite;

    public Enums.CardType Type;
    public Enums.Class Class;
    public Enums.RarityType Rarity;
    public Enums.ModifierType Level;
    public bool ExhaustsBase = false;
    public bool ExhaustsEnhanced = false;

    [SerializeField, SerializeReference]
    public List<Effect> Effects;



#if UNITY_EDITOR
    [PropertyOrder(-1)]
    [Button("Generate Id", ButtonSizes.Medium, 0)]
    protected override void GenerateUniqueId()
    {
        string prefix = "C";
        int id = 1;

        string[] existingAssetPaths = AssetDatabase.FindAssets("t:CardData", new[] { "Assets/Data/Cards" });
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

    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }

    public static CardData CopyOf(CardData cardToCopy)
    {

        CardData card = CreateInstance<CardData>();
        card.Id = cardToCopy.Id;
        card.Name = cardToCopy.Name;
        card.Description = cardToCopy.Description;
        card.CardSprite = cardToCopy.CardSprite;
        card.BorderSprite = cardToCopy.BorderSprite;
        card.Cost = cardToCopy.Cost;
        card.Type = cardToCopy.Type;
        card.Class = cardToCopy.Class;
        card.Rarity = cardToCopy.Rarity;
        card.Level = cardToCopy.Level;
        card.Effects = new List<Effect>();
        foreach (var effect in cardToCopy.Effects)
        {
            card.Effects.Add(effect);
        }
        return card;
    }

    public void Enchant()
    {
        Level = Enums.ModifierType.Enchanted;
        foreach (var effect in Effects)
        {
            effect.Level = Level;
        }
    }

    public bool Exhausts()
    {
        return (Level == Enums.ModifierType.Base) ? ExhaustsBase : ExhaustsEnhanced;
    }
}
