using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BattleController : MonoBehaviour
{
    public static BattleController Instance;
    [SerializeField] Player _player;
    [SerializeField] GameObject _enemyPrefab;
    RoomManager _roomManager;
    EncounterData _encounter;
    List<Enemy> currentEnemies;

    public Transform EnemySpawnPoint;
    public GameObject EnergyUI;
    public GameObject EndTurnButton;
    public UnityEvent<Enemy> EnemyClickedEvent;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    public ITarget GetRandomEnemy(Enemy enemy)
    {
        if (currentEnemies.Count > 1)
        {
            var enemies = currentEnemies.Where(e => e != enemy).ToList();
            return enemies[Random.Range(0, enemies.Count)];
        }
        else return enemy;
    }

    public List<Enemy> GetEnemies()
    {
        return currentEnemies;
    }

    public void GenerateFight(RoomManager roomManager, Enums.NodeType type)
    {
        _roomManager = roomManager;
        _encounter = DataManager.Instance.GetEncounter(type);
        SetupBattle();
    }

    private void SetupBattle()
    {
        currentEnemies = new List<Enemy>();
        foreach (EnemyData enemy in _encounter.enemies)
        {
            Enemy newEnemy = Instantiate(_enemyPrefab, EnemySpawnPoint).GetComponent<Enemy>();
            newEnemy.Setup(this, enemy, _player);
            newEnemy.PlanNextAttack();
            currentEnemies.Add(newEnemy);
        }
        _player.SetDeckForBattle();
        PlayerTurn();
    }

    private void PlayerTurn()
    {
        EnergyUI.SetActive(true);
        EndTurnButton.SetActive(true);
        _player.StartTurn();
    }

    private void EnemyTurn()
    {
        foreach (Enemy enemy in currentEnemies)
        {
            enemy.ResetDefense();
        }
        foreach (Enemy enemy in currentEnemies)
        {
            enemy.ExecuteIntendedAttack();
            if (_player.IsDefeated())
            {
                EndEnemyTurn();
                return;
            }
        }
        EndEnemyTurn();
    }

    public void EndPlayerTurn()
    {
        _player.DiscardHand();
        EnergyUI.SetActive(false);
        EndTurnButton.SetActive(false);

        if (currentEnemies.Count == 0)
            EndBattle(true);
        else
            EnemyTurn();
    }

    public void EndEnemyTurn()
    {
        if (_player.IsDefeated())
        {
            EndBattle(false);
            return;
        }
        foreach (Enemy enemy in currentEnemies)
        {
            enemy.PlanNextAttack();
        }
        PlayerTurn();
    }

    public void OnEnemyDeath(Enemy enemy)
    {
        currentEnemies.Remove(enemy);
        if (currentEnemies.Count == 0)
            EndPlayerTurn();
    }

    private void EndBattle(bool victory)
    {
        if (victory) // Victory
        {
            _roomManager.GenerateNewChest(_encounter.Type);
        }
        else // Defeat
        {
            ResetArena();
            GameManager.Instance.EndRun();
        }
    }

    private void ResetArena()
    {
        _player.DiscardHand();
        var enemies = currentEnemies.Count;
        for (int i = 0; i < enemies; i++)
        {
            Destroy(currentEnemies[0].gameObject);
            currentEnemies.RemoveAt(0);
        }
    }

    public bool HasBattleEnded()
    {
        return _player.IsDefeated() || currentEnemies.Count == 0;
    }
}
