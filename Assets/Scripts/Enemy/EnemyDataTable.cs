using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Enemy Data Table")]
public class EnemyDataTable : ScriptableObject
{
    public EnemyInfo[] enemy;

    private int curIndex = -1;
    private int savedVal = -1;

    public EnemyInfo GetRandomEnemy(int index)
    {
        index = Mathf.Clamp(index, 0, enemy.Length);
        if (index != curIndex)
        {
            curIndex = index;
            savedVal = 0;
            for (int i = 0; i < index; i++)
            {
                savedVal += enemy[i].weight;
            }
        }

        int rng = Random.Range(0, savedVal) + 1;
        for (int i = 0; i < index; i++)
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
