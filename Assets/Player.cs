using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, ITarget
{
    [SerializeField] Image _playerImage;
    [SerializeField] GeneralUI _generalUI;
    [SerializeField] DeckManager _deckManager;
    [SerializeField] public BattleController _battleController;
    int _maxHealth;
    int _health;
    int _defense;
    int _maxEnergy;
    int _energy;
    public int Gold { get; set; }
    ClassData _class;
    Stats _stats;


    public void Setup(ClassData classData)
    {
        _class = classData;
        _stats = classData.BaseStats;
        _maxEnergy = classData.BaseEnergy;
        _maxHealth = classData.BaseHealth + classData.BaseHealth * _stats.ConMod;
        _health = _maxHealth;
        _energy = _maxEnergy;
        Gold = 0;
        _playerImage.sprite = classData.Sprite;
        _generalUI.Setup(classData);
    }


    public void UpdateGeneralUI()
    {
        _generalUI.UpdateVisuals(_maxHealth, _health, _defense, _energy, Gold, _stats, _deckManager.DrawPile.Count, _deckManager.DiscardPile.Count);
    }

    public bool IsDefeated()
    {
        return _health <= 0;
    }

    public void StartTurn()
    {
        _defense = 0;
        ResetEnergy();
        DrawHand();
    }

    public void ResetEnergy()
    {
        _energy = _maxEnergy;
        UpdateGeneralUI();
    }

    #region Hand Methods

    public void DrawHand()
    {
        _deckManager.DrawHand();
    }

    public void DiscardHand()
    {
        _deckManager.DiscardHand();
    }
    #endregion

    #region ITarget methods
    public void AddDefense(int amount)
    {
        _defense += amount;
        UpdateGeneralUI();
    }

    public void DealDamage(int amount)
    {
        _health -= Mathf.Max(amount - _defense, 0);
        _defense = Mathf.Max(_defense - amount, 0);
        UpdateGeneralUI();
    }

    public void Heal(int amount)
    {
        _health = Mathf.Min(_maxHealth, _health + amount);
        UpdateGeneralUI();

    }

    public bool IsPlayable(CardData cardData)
    {
        return _energy - cardData.Cost >= 0;
    }

    public int GetMaxHealth()
    {
        return _maxHealth;
    }

    public ClassData GetClass()
    {
        return _class;
    }

    public void PlayCard(CardDisplay card, ITarget source, ITarget target)
    {
        var effects = card._cardData.Effects;
        _energy -= card._cardData.Cost;
        foreach (var effect in effects)
        {

            effect.Apply(source, target, card._cardData.Level, _stats);

        }
        UpdateGeneralUI();
        _deckManager.PlayCard(card);
    }

    public Stats GetStats()
    {
        return _stats;
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        UpdateGeneralUI();
    }

    public void SubtractGold(int amount)
    {
        Gold -= amount;
        UpdateGeneralUI();
    }

    public void SetDeckForBattle()
    {
        _deckManager.SetForBattle();
    }


    #endregion

}
