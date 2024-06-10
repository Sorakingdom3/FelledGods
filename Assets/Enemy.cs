using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : Target, IPointerClickHandler
{
    BattleController _controller;
    [SerializeField] EnemyUI _enemyUI;

    EnemyData _enemyData;
    string _enemyName;

    Target _player;
    List<Effect> _attacks;
    Effect _intendedAction;
    List<Target> _intendedTargets = new List<Target>();

    // Puedes añadir más atributos según las necesidades de tu juego

    public void Setup(BattleController battleController, EnemyData enemyData, Target player)
    {
        _controller = battleController;
        _enemyName = enemyData.Name;
        _player = player;
        _enemyData = enemyData;
        _stats = _enemyData.Stats;
        _maxHealth = _enemyData.BaseHealth;
        _health = _maxHealth;
        _attacks = _enemyData.Attacks;
        _enemyUI.Set(_enemyData);

    }

    // Método para ejecutar acciones cuando el enemigo muere
    private void Die()
    {
        // Implementa aquí cualquier lógica que desees ejecutar cuando el enemigo muere
        Destroy(gameObject); // Por ejemplo, destruir el objeto del enemigo
        _controller.OnEnemyDeath(this);
    }

    public void ExecuteIntendedAttack()
    {
        foreach (var target in _intendedTargets)
        {
            if (target != null)
                _intendedAction.Apply(this, target, _stats);
            if (_intendedAction is DamageEffect)
                DealDamage();
        }
        _intendedTargets.Clear();
    }

    public override void AddDefense(int amount)
    {
        base.AddDefense(amount);
        _enemyUI.UpdateVisuals(_health, _maxHealth, _defense);
    }

    public override bool RecieveDamage(int amount)
    {
        base.RecieveDamage(amount);
        _enemyUI.UpdateVisuals(_health, _maxHealth, _defense);
        if (_health <= 0)
        {
            Die();
            return true;
        }
        return false;
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
        _enemyUI.UpdateVisuals(_health, _maxHealth, _defense);
    }

    public void PlanNextAttack()
    {
        var attacks = new List<Effect>();
        if (_intendedAction != null)
            attacks = _attacks.Where(a => a != _intendedAction).ToList();
        else
            attacks = _attacks;
        _intendedAction = attacks[Random.Range(0, attacks.Count)];
        switch (_intendedAction.TargetType)
        {
            case Enums.TargetType.SingleEnemy:
            case Enums.TargetType.MultipleEnemies:
                _intendedTargets.Add(_player);
                break;
            case Enums.TargetType.Self:
                _intendedTargets.Add(this);
                break;
        }
        _enemyUI.SetIntention(_intendedAction, _stats);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _controller.EnemyClickedEvent?.Invoke(this);
    }

    public void ResetDefense()
    {
        _defense = 0;
        _enemyUI.UpdateVisuals(_health, _maxHealth, _defense);
    }

    public void StartTurn()
    {
        var activeBuffs = Buffs.Keys;
        List<Enums.BuffType> removed = new List<Enums.BuffType>();
        foreach (var buff in activeBuffs)
        {
            var effect = Buffs[buff];
            effect.HandleDuration();
            if (effect.HasExpired())
            {
                removed.Add(buff);
                _enemyUI.RemoveBuff(buff);
            }
            else
            {
                if (effect.HasGrowth())
                {
                    effect.ApplyGrowth(this);
                }
                _enemyUI.UpdateBuff(effect);
            }
        }
        foreach (var removedBuff in removed)
        {
            Buffs.Remove(removedBuff);
        }

        ResetDefense();
    }

    public override void UpdateBuffUI(bool isNew, ModifierEffect effect)
    {
        if (isNew)
        {
            _enemyUI.AddNewBuff(effect);
        }
        else
        {
            _enemyUI.UpdateBuff(effect);
        }
    }
}
