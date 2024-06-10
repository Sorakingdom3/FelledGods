using System.Collections.Generic;
using UnityEngine;

public abstract class Target : MonoBehaviour
{
    [SerializeField] Animator _animator;

    protected Stats _stats;
    protected int _maxHealth;
    protected int _health;
    protected int _defense;
    public Dictionary<Enums.BuffType, ModifierEffect> Buffs = new Dictionary<Enums.BuffType, ModifierEffect>();

    /// <summary>
    /// Character increases defense
    /// </summary> 
    public virtual void AddDefense(int amount)
    {
        int defense = _defense;

        if (Buffs.ContainsKey(Enums.BuffType.Sturdy))
        {
            defense += Buffs[Enums.BuffType.Sturdy].GetAmount() + amount;
        }
        else
        {
            defense += amount;
        }
        if (Buffs.ContainsKey(Enums.BuffType.Fragile))
        {
            defense = Mathf.FloorToInt(defense * .75f);
        }
        if (defense > 0)
            _animator.SetTrigger("Defense");
        _defense = defense;
    }

    /// <summary>
    /// Character receives Damage
    /// </summary> 
    public virtual bool RecieveDamage(int amount)
    {
        if (Buffs.ContainsKey(Enums.BuffType.Vulnerable))
        {
            amount = Mathf.FloorToInt(amount * 1.5f);
        }

        _animator.SetTrigger("Damage");

        _health -= Mathf.Max(amount - _defense, 0);
        _health = Mathf.Max(_health, 0);
        _defense = Mathf.Max(_defense - amount, 0);

        if (_health <= 0)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Character heals
    /// </summary> 
    public virtual void Heal(int amount)
    {
        if (Buffs.ContainsKey(Enums.BuffType.Nurtured))
        {
            amount += Buffs[Enums.BuffType.Nurtured].GetAmount();
        }

        if (Buffs.ContainsKey(Enums.BuffType.Malnourished))
        {
            amount /= 2;
        }

        if (amount > 0)
            _animator.SetTrigger("Heal");

        _health = Mathf.Min(_maxHealth, _health + amount);
    }
    /// <summary>
    /// Character recieves a Buff
    /// </summary> 
    public virtual void ApplyModification(ModifierEffect effect)
    {
        switch (effect.GetBuffType())
        {
            case Enums.BuffType.Strength:
            case Enums.BuffType.Nurtured:
            case Enums.BuffType.Sturdy:
            case Enums.BuffType.Rage:
                _animator.SetTrigger("Buff");
                break;
            default:
                _animator.SetTrigger("Debuff");
                break;
        }

        if (!Buffs.ContainsKey(effect.GetBuffType()))
        {
            Buffs.Add(effect.GetBuffType(), effect);
            UpdateBuffUI(true, effect);
        }
        else
        {
            Buffs[effect.GetBuffType()].Stack(effect);
            UpdateBuffUI(false, effect);
        }
    }

    public virtual void DealDamage()
    {
        _animator.SetTrigger("Attack");
    }

    public abstract void UpdateBuffUI(bool isNew, ModifierEffect effect);
}