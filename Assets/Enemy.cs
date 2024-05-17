using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour, ITarget, IPointerClickHandler
{
    EnemyData _enemyData;
    string _enemyName;
    int _maxHealth;
    int _health;
    int _defense;


    Stats _stats;
    List<Effect> _attacks;
    Effect _intendedAction;
    List<ITarget> _intendedTargets = new List<ITarget>();
    ITarget _player;
    BattleController _controller;

    [SerializeField] EnemyUI _enemyUI;
    // Puedes añadir más atributos según las necesidades de tu juego

    public void Setup(BattleController battleController, EnemyData enemyData, ITarget player)
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
                _intendedAction.Apply(this, target, Enums.ModifierType.Base, _stats);
        }
        _intendedTargets.Clear();
        _intendedAction = null;
    }

    public void AddDefense(int amount)
    {
        _defense += amount;
        _enemyUI.UpdateVisuals(_health, _maxHealth, _defense);
    }

    public void DealDamage(int amount)
    {
        _health -= Mathf.Max(amount - _defense, 0);
        _defense = Mathf.Max(_defense - amount, 0);
        _enemyUI.UpdateVisuals(_health, _maxHealth, _defense);
        if (_health <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        _health += amount;
        _health = Mathf.Min(_health, _maxHealth);
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
}
