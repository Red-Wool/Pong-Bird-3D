using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class EnemyTemplate : MonoBehaviour
{
    private GameObject target; public GameObject Target { get { return target; } }

    public virtual void Reset(Vector3 pos, GameObject t)
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 1f).SetEase(Ease.OutCubic);

        transform.position = pos;
        target = t;
    }
}
