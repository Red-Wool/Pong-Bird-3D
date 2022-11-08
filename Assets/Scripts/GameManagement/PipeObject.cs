using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PipeObject : MonoBehaviour
{
    [SerializeField] private Vector3 pipeSize;

    public void PipeAnimation(bool flag)
    {
        if (flag)
        {
            transform.localScale = Vector3.up * pipeSize.y;
            transform.DOScale(pipeSize, 3f);
        }
        else
        {
            //gameObject.SetActive(false);
            transform.DOScale(Vector3.up * pipeSize.y, 1f);
            StartCoroutine(DisablePipe());
        }
    }

    private IEnumerator DisablePipe()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
