using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ExtraJump : ItemScript
{
    public override void Use(GameModify mod)
    {
        mod.moveStat.maxJump += 1;
    }
}
