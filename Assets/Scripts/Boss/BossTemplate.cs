using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class BossTemplate : EnemyTemplate
{
    protected int phase;
    [SerializeField] protected int finalPhase;

    [SerializeField] protected BossAttack[] attack;
    [SerializeField] protected BossPaddleContainer[] weakPoints;

    public override void Reset(Vector3 pos, GameObject t)
    {
        base.Reset(pos, t);

        EnableContainer(0);
        phase = 0;
    }

    public virtual void Damage()
    {
        phase++;
        if (phase >= finalPhase)
        {
            Death();
        }
        else
            EnableContainer(phase);
    }

    public void EnableContainer(int v)
    {
        for(int i = 0; i < weakPoints.Length; i++)
        {
            if (i == v)
            {
                weakPoints[i].Enable(this);
            }
            else
                weakPoints[i].Disable(true);
        }
    }

    public virtual void Tick(float tick)
    {
        for (int i = 0; i < attack.Length; i++)
        {
            BossAttack a = attack[i];
            if (phase >= a.phase.x && phase <= a.phase.y)
            {
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
                attack[i] = a;
            }
        }
    }

    public virtual void Death()
    {
        Debug.Log("Dead");
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
