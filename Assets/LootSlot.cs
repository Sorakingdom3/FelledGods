using TMPro;
using UnityEngine;

public class LootSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _Text;
    public void SetUp(Enums.LootType type, string name, string description, int amount)
    {
        switch (type)
        {
            case Enums.LootType.Gold:
                _Text.text = $"<sprite=0> {amount} Gold";
                break;
            case Enums.LootType.Relic:
                _Text.text = $"<sprite=0> {amount} Gold";
                break;
            case Enums.LootType.Cards:

                break;
            case Enums.LootType.Potion:

                break;
        }
    }
}
