using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyTemplate : MonoBehaviour
{
    private GameObject target; public GameObject Target { get { return target; } }

    public virtual void Reset(Vector3 pos, GameObject t)
    {
        transform.position = pos;
        target = t;
    }
}
