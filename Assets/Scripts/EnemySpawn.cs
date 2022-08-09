using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private ObjectPool orangeDrill;
    [SerializeField] private float enemySpawnTick;
    [SerializeField] private float timer;

    [SerializeField] private float timeScale;
    [SerializeField] private float pointBaseScale;
    [SerializeField] private float pointTimeMult;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    public void SummonEnemies()
    {
        float dangerLvl = timeScale * (1 + pointTimeMult * GameManager.Score) * timer + GameManager.Score * pointBaseScale;
        int enemyPts = (int)Random.Range(dangerLvl * 0.2f, dangerLvl * 0.6f);


        for (int i = 0; i < enemyPts; i++)
        {

        }
    }
}
