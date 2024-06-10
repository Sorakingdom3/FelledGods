using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Target
{
    [SerializeField] Image _playerImage;
    [SerializeField] GeneralUI _generalUI;
    [SerializeField] DeckManager _deckManager;
    [SerializeField] public BattleController _battleController;
    ClassData _class;
    int _maxEnergy;
    int _energy;
    public int Gold { get; set; }

    public void Setup(ClassData classData)
    {
        _class = classData;
        _stats = classData.BaseStats;
        _maxEnergy = classData.BaseEnergy;
        _maxHealth = classData.BaseHealth;
        _health = _maxHealth;
        _energy = _maxEnergy;
        Gold = classData.BaseGold;
        _playerImage.sprite = classData.Sprite;
        _generalUI.Setup(classData);
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
    public int GetMaxHealth()
    {
        return _maxHealth;
    }
    public ClassData GetClass()
    {
        return _class;
    }
    public Stats GetStats()
    {
        return _stats;
    }
    public void UpdateGeneralUI()
    {
        _generalUI.UpdateVisuals(_maxHealth, _health, _defense, _energy, Gold, _stats, _deckManager.DrawPile.Count, _deckManager.DiscardPile.Count);
    }

    #region BattleMethods

    public void DrawCard()
    {
        _deckManager.DrawCard();
    }
    public bool IsDefeated()
    {
        return _health <= 0;
    }
    public void StartTurn()
    {
        var activeBuffs = Buffs.Keys;
        List<Enums.BuffType> buffKeys = new List<Enums.BuffType>();
        foreach (var buff in activeBuffs)
        {
            buffKeys.Add(buff);
        }
        foreach (var buff in buffKeys)
        {
            var effect = Buffs[buff];
            effect.HandleDuration();

            if (effect.HasExpired())
            {
                Buffs.Remove(buff);
                _generalUI.RemoveBuff(buff);
            }
            else
            {
                if (effect.HasGrowth())
                {
                    effect.ApplyGrowth(this);
                }
                _generalUI.UpdateBuff(effect);
            }
        }
        _defense = 0;
        ResetEnergy();
        DrawHand();
    }
    public void ResetEnergy()
    {
        _energy = _maxEnergy;
        UpdateGeneralUI();
    }
    public void DrawHand()
    {
        _deckManager.DrawHand();
    }
    public void DiscardHand()
    {
        _deckManager.DiscardHand();
    }
    public bool IsPlayable(CardData cardData)
    {
        return _energy - cardData.Cost >= 0;
    }
    public void StartCombat()
    {
        _deckManager.SetForBattle();
    }
    public void OnBattleEnd()
    {
        Buffs.Clear();
        _generalUI.ClearBuffs();
    }
    public void PlayCard(CardDisplay card, Target source, Target target)
    {
        var effects = card._cardData.Effects;
        _energy -= card._cardData.Cost;
        foreach (var effect in effects)
        {
            if (!_battleController.HasBattleEnded())
            {
                if (effect is DamageEffect)
                {
                    DealDamage();
                }
                effect.Apply(source, target, _stats);
            }
        }
        if (IsDefeated())
        {
            _battleController.OnPlayerDeath();
            return;
        }
        UpdateGeneralUI();
        _deckManager.PlayCard(card);
    }

    #endregion

    #region ITarget methods

    public override void AddDefense(int amount)
    {
        base.AddDefense(amount);
        UpdateGeneralUI();
    }

    public override bool RecieveDamage(int amount)
    {
        bool result = base.RecieveDamage(amount);
        UpdateGeneralUI();
        return result;
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
        UpdateGeneralUI();

    }

    public override void ApplyModification(ModifierEffect effect)
    {
        base.ApplyModification(effect);
    }

    public override void UpdateBuffUI(bool isNew, ModifierEffect effect)
    {
        if (isNew)
        {
            _generalUI.AddNewBuff(effect);
        }
        else
        {
            _generalUI.UpdateBuff(effect);
        }
    }

    public void AddEnergy(int energyAmount)
    {
        _energy += energyAmount;
        _generalUI.UpdateVisuals(_maxHealth, _health, _defense, _energy, Gold, _stats, _deckManager.DrawPile.Count, _deckManager.DiscardPile.Count);
    }

    #endregion
}
