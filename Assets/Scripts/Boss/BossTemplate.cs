using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossTemplate : EnemyTemplate
{
    protected int phase;
    protected int finalPhase;

    [SerializeField] protected BossAttack[] attack;

    public override void Reset(Vector3 pos, GameObject t)
    {
        base.Reset(pos, t);

        phase = 0;
    }

    public virtual void Damage()
    {
        phase++;
        if (phase >= finalPhase)
        {
            Death();
        }
    }

    public virtual void Tick(float tick)
    {
        for (int i = 0; i < attack.Length; i++)
        {
            BossAttack a = attack[i];
            a.time -= tick;
            if (a.time < 0)
            {
                a.attackLeft--;
                if (a.attackLeft <= 0)
                {
                    a.attackLeft = a.repeatCount;
                    a.time = a.reload;
                }
                else
                {
                    a.time = a.repeatInterval;
                }

                a.ps.Play();
            }
        }
    }

    public virtual void Death()
    {

    }
}

[System.Serializable]
public struct BossAttack
{
    public ParticleSystem ps;
    public Vector2 phase;

    public float init;
    public float reload;
    public Vector2 randTime;
    public Vector2 minMaxDist;

    public bool target;
    public Vector3 randAng;

    public int repeatCount;
    public float repeatInterval;

    [HideInInspector] public float time;
    [HideInInspector] public int attackLeft;
}
