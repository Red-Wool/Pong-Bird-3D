using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatformLocation : MonoBehaviour
{
    [SerializeField] private GameObject currentObj = null;

    private IEnumerator anim;
    private bool finish = false;

    private void Start()
    {
        currentObj = null;
    }

    public void Reset()
    {
        
    }

    public void Spawn(PlatformSpawn platform)
    {
        if (finish)
        {
            StopCoroutine(anim);
        }

        anim = SpawnAnimation(platform);
        StartCoroutine(anim);
    }

    public IEnumerator SpawnAnimation(PlatformSpawn platform)
    {
        finish = true;

        if (currentObj != null)
            currentObj.transform.DOMoveY(-100f, 1f).SetEase(Ease.InOutQuint);

        yield return new WaitForSeconds(Random.Range(.5f, 2f));

        if (currentObj != null)
            currentObj.SetActive(false);

        currentObj = platform.prefab.GetObject();
        currentObj.transform.position = transform.position + Vector3.down * 100;
        currentObj.transform.DOMove(transform.position + UtilFunctions.RandomV3(platform.randomSpawnRange), Random.Range(1f, 3f)).SetEase(Ease.InOutQuint);

        finish = false;


    }

    public IEnumerator RemoveAnimation()
    {
        finish = true;
        if (currentObj != null)
            currentObj.transform.DOMoveY(-100f, 1f).SetEase(Ease.InOutQuint);

        yield return new WaitForSeconds(Random.Range(.5f, 2f));

        if (currentObj != null)
            currentObj.SetActive(false);

        finish = false;
    }

    public void Remove()
    {
        if (finish)
        {
            StopCoroutine(anim);
        }
        
        if (currentObj != null)
            currentObj.SetActive(false);
    }
}