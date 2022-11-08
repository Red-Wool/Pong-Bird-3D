using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPaddleContainer : MonoBehaviour
{
    [SerializeField] private BossPaddle[] paddles;

    private BossTemplate boss;

    public void Enable(BossTemplate b)
    {
        Debug.Log(gameObject.name);
        boss = b;
        foreach(BossPaddle p in paddles)
        {
            p.Reset(this);
        }
    }

    public void Disable(bool show)
    {
        foreach (BossPaddle p in paddles)
        {
            //p.Reset(this);
        }
    }

    public void PaddleHit()
    {
        foreach(BossPaddle p in paddles)
        {
            if (p.isActive)
            {
                return;
            }
        }

        boss.Damage();
    }
}
