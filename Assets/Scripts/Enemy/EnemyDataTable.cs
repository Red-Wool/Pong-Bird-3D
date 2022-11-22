using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Enemy Data Table")]
public class EnemyDataTable : ScriptableObject
{
    public float enemySpawnTick;

    public float timeScale;

    public float timeScaleStart;
    public float timeScalePointMult;
    public float timeScaleCap;

    public float pointBaseScale;

    public EnemySpawnData[] enemy;
    public EnemyBulletAttack[] attacks;

    private int curIndex = -1;
    private int savedVal = -1;

    public int CheckTable(int round)
    {
        savedVal = 0;
        EnemySpawnData s;
        for (int i = 0; i < enemy.Length; i++)
        {
            s = enemy[i];
            if (s.round <= round)
            {
                if (s.round == round && s.isBoss)
                {
                    return i;
                }

                curIndex = i;
                savedVal += enemy[i].weight;
            }
            else
                break;
        }
        return -1;
    }

    public EnemySpawnData GetRandomEnemy(int round)
    {
        int rng = Random.Range(0, savedVal) + 1;
        //Debug.Log(rng + " Rng");
        for (int i = 0; i <= curIndex; i++)
        {
            rng -= enemy[i].weight;

            if (rng <= 0)
            {
                return enemy[i];
            }
        }

        return enemy[0];
    }
}
