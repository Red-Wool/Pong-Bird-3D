using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private EnemyDataTable enemies;
    [SerializeField] private GameObject player;

    [SerializeField] private float timer;
    private float nextTick;
    private bool fightBoss;

    private int enemyIndex;

    public static UnityEvent bossSummon;

    public void Awake()
    {
        bossSummon = new UnityEvent();
    }

    // Start is called before the first frame update
    void Start()
    {
        

        PaddleObject.PointScored += ResetTimer;
        PlayerMove.death.AddListener(ResetTimer);

        foreach(EnemySpawnData e in enemies.enemy)
        {
            e.enemyPrefab.AddObjects();
        }
        foreach(EnemyBulletAttack a in enemies.attacks)
        {
            a.Reset();
        }
    }

    public void ResetTimer()
    {
        timer = 0;
        nextTick = 0;

        fightBoss = false;

        int bossNum = enemies.CheckTable(GameManager.Score);
        if (bossNum != -1)
        {
            
            SummonBoss(enemies.enemy[bossNum]);
        }
    }

    public void SummonBoss(EnemySpawnData boss)
    {
        fightBoss = true;
        bossSummon.Invoke();
        boss.enemyPrefab.GetObject();
    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameStart && !fightBoss)
        {
            timer += Time.deltaTime;
            if (timer > nextTick)
            {
                nextTick += enemies.enemySpawnTick;
                SummonEnemies();
            }
        }
        
    }

    public void SummonEnemies()
    {
        float dangerLvl = Mathf.Min(enemies.timeScale * timer, Mathf.Clamp(enemies.timeScalePointMult * GameManager.Score,enemies.timeScaleStart,enemies.timeScaleCap)) + Mathf.Min(GameManager.Score * enemies.pointBaseScale, 100);
        int enemyPts = Mathf.Clamp((int)Random.Range(dangerLvl * 0.2f, dangerLvl * 0.6f), 0, 30);
        
        for (int i = 0; i < enemyPts; i++)
        {
            EnemySpawnData e = enemies.GetRandomEnemy(GameManager.Score);

            Vector3 spawnLocation = Vector3.zero;
            switch (e.type)
            {
                case EnemyType.Perimeter:
                    spawnLocation = Random.onUnitSphere;
                    spawnLocation.y = 0f;
                    spawnLocation = spawnLocation.normalized * 178 + Vector3.up * Random.Range(-5f, 50f);
                    break;
                case EnemyType.Ground:
                    spawnLocation = Random.onUnitSphere * 120;
                    spawnLocation.y = -15f;
                    break;
                case EnemyType.Pipe:
                    break;
            }

            GameObject enemy = e.enemyPrefab.GetObject();
            enemy.GetComponent<EnemyTemplate>().Reset(spawnLocation, player);
        }
    }
}

[System.Serializable]
public struct EnemySpawnData
{
    public int round;
    public bool isBoss;

    public ObjectPool enemyPrefab;
    public int weight;

    public EnemyType type;
}

public enum EnemyType
{
    Perimeter,
    Ground,
    Pipe
}
