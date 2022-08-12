using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider))]
public class BreakingPlatform : PlatformTemplate
{
    [SerializeField] private GameObject visual;
    [SerializeField] private Collider hitBox;
    public override void Reset()
    {
        hitBox.enabled = true;
        visual.transform.localPosition = Vector3.zero;
        visual.transform.DOScale(1, .5f);

    }

    public IEnumerator Breakdown()
    {
        visual.transform.DOShakePosition(1.5f, 1, 10);
        yield return new WaitForSeconds(1.5f);
        visual.transform.DOScale(0, .1f);
        hitBox.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            StartCoroutine(Breakdown());
        }
    }
}
