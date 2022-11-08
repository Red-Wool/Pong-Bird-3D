using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gameplay/Enemy Bullet Attack")]
public class EnemyBulletAttack : ScriptableObject
{
    public ObjectPool attack;

    public void Reset()
    {
        
        attack.Reset();
    }

    public void SpawnAttack(Vector3 pos, Quaternion rot)
    {
        if (attack.Pool.Count == 0)
        {
            attack.AddObjects();
        }
        foreach (GameObject o in attack.Pool)
        {
            ParticleSystem p = o.GetComponent<ParticleSystem>();
            if (!p.isPlaying)
            {
                //Debug.Log("Bang!");
                o.transform.position = pos;
                o.transform.rotation = rot;

                o.SetActive(true);
                p.Play();
                return;
            }
        }

        attack.AddObjects();
        SpawnAttack(pos, rot);
    }
}
