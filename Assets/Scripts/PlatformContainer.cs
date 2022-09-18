using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformContainer : PlatformTemplate
{
    [SerializeField] private PlatformTemplate[] other;

    public override void Reset()
    {
        foreach(PlatformTemplate p in other)
        {
            p.Reset();
        }
    }
}
