using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatformLocation : MonoBehaviour
{
    [SerializeField] private GameObject currentObj = null;
    private bool hasPlatform = false; public bool HasPlatform { get { return hasPlatform; } }

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
        {
            currentObj.transform.DOShakePosition(.5f, 3, 20, 90);
            currentObj.transform.DOMoveY(-100f, 1f).SetEase(Ease.InOutQuint);
        }
           

        yield return new WaitForSeconds(Random.Range(.5f, 2f));

        if (currentObj != null)
            currentObj.SetActive(false);

        currentObj = platform.prefab.GetObject();

        if (currentObj != null)
        {
            PlatformTemplate tp = currentObj.GetComponent<PlatformTemplate>();
            if (tp != null)
            {
                tp.Reset();
            }
        }

        currentObj.transform.rotation = Quaternion.Euler(0,Random.Range(0, 4) * 90f,0);

        currentObj.transform.position = transform.position + Vector3.down * 100;
        currentObj.transform.DOMove(transform.position + UtilFunctions.RandomV3(platform.randomSpawnRange), Random.Range(1f, 3f)).SetEase(Ease.InOutQuint);

        hasPlatform = true;
        finish = false;


    }

    public IEnumerator RemoveAnimation()
    {
        if (currentObj != null)
        {
            currentObj.transform.DOShakePosition(1f, 3, 20, 90);
        }

        yield return new WaitForSeconds(Random.Range(.5f, 1f));

        finish = true;
        if (currentObj != null)
            currentObj.transform.DOMoveY(-100f, 1f).SetEase(Ease.InOutQuint);

        yield return new WaitForSeconds(Random.Range(.5f, 2f));

        if (currentObj != null)
            currentObj.SetActive(false);

        currentObj = null;
        hasPlatform = false;

        finish = false;
    }

    public void Remove()
    {
        if (finish)
        {
            StopCoroutine(anim);
        }

        if (currentObj != null && currentObj.activeSelf)
            StartCoroutine(RemoveAnimation());
            //currentObj.SetActive(false);
    }
}