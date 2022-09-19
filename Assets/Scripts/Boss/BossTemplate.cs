using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossTemplate : EnemyTemplate
{
    protected int phase;
    protected int finalPhase;

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

    public virtual void Death()
    {

    }
}
