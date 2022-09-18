using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private EnemyDataTable enemies;
    [SerializeField] private GameObject player;

    [SerializeField] private float timer;
    private float nextTick;

    private int enemyIndex;
    // Start is called before the first frame update
    void Start()
    {

        PaddleObject.PointScored += ResetTimer;
        PlayerMove.death.AddListener(ResetTimer);

        foreach(EnemyInfo e in enemies.enemy)
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
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameStart)
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
            EnemyInfo e = enemies.GetRandomEnemy(GameManager.Score / 5);

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
public struct EnemyInfo
{
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
