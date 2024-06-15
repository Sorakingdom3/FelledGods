using System.Collections;
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

    public List<Transform> EnemySpawnPoints;
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

    public Target GetRandomEnemy(Enemy enemy)
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
        var count = _encounter.enemies.Count;
        List<Transform> spawnPoints;
        if (count == 1)
            spawnPoints = new List<Transform> { EnemySpawnPoints[2] };
        else if (count == 2)
            spawnPoints = new List<Transform> { EnemySpawnPoints[1], EnemySpawnPoints[3] };
        else
            spawnPoints = new List<Transform> { EnemySpawnPoints[0], EnemySpawnPoints[2], EnemySpawnPoints[4] };
        int i = 0;
        foreach (EnemyData enemy in _encounter.enemies)
        {
            Enemy newEnemy = Instantiate(_enemyPrefab, spawnPoints[i]).GetComponent<Enemy>();
            newEnemy.Setup(this, enemy, _player);
            newEnemy.PlanNextAttack();
            currentEnemies.Add(newEnemy);
            i++;
        }
        _player.StartCombat();
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
            enemy.StartTurn();
        }
        StartCoroutine(DoEnemiesAttackAnimations());
    }

    private IEnumerator DoEnemiesAttackAnimations()
    {
        foreach (Enemy enemy in currentEnemies)
        {
            enemy.ExecuteIntendedAttack();
            yield return new WaitForSeconds(1);
            if (_player.IsDefeated())
            {
                EndEnemyTurn();
                yield break;
            }
        }
        EndEnemyTurn();
        yield return null;
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
    public void OnPlayerDeath()
    {
        _player.DiscardHand();
        EnergyUI.SetActive(false);
        EndTurnButton.SetActive(false);

        EndBattle(false);
    }

    public void OnEnemyDeath(Enemy enemy)
    {
        currentEnemies.Remove(enemy);
        if (currentEnemies.Count == 0)
            EndPlayerTurn();
    }

    private void EndBattle(bool victory)
    {
        _player.OnBattleEnd();
        if (victory) // Victory
        {
            _roomManager.GenerateNewChest(_encounter.Type);
        }
        else // Defeat
        {
            ResetArena();
            GameManager.Instance.EndRun(false);
        }
    }

    public void ResetArena()
    {
        _player.DiscardHand();
        if (currentEnemies != null)
        {
            var enemies = currentEnemies.Count;
            for (int i = 0; i < enemies; i++)
            {
                Destroy(currentEnemies[0].gameObject);
                currentEnemies.RemoveAt(0);
            }
        }
    }

    public bool HasBattleEnded()
    {
        return _player.IsDefeated() || currentEnemies.Count == 0;
    }

}
