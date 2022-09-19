using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawn : MonoBehaviour
{
    [SerializeField] private ObjectPool pipePool;
    [SerializeField] private Vector3 angleOffset;
    [SerializeField] private Vector2 spawnNum;
    public float spawnradius = 30;
    public float spawnCollisionCheck;
    void Start()
    {
        pipePool.AddObjects();
        SpawnPipes((int)spawnNum.x);
        PaddleObject.PointScored += SetUpPipes;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpPipes()
    {
        SpawnPipes((int)Random.Range(spawnNum.x, spawnNum.y));
    }

    private void SpawnPipes(int count)
    {
        GameObject obj = null;
        for (int i = 0; i < pipePool.Pool.Count; i++)
        {
            if (pipePool.Pool[i].activeInHierarchy)
            {
                pipePool.Pool[i].GetComponent<PipeObject>().PipeAnimation(false);
            }
        }

        for (int loop = 0; loop < count; loop++)
        {
            Vector3 spawnPoint = transform.position + Random.insideUnitSphere * spawnradius;

            if (!Physics.CheckSphere(spawnPoint, spawnCollisionCheck))
            {
                obj = pipePool.GetObject();
                obj.transform.position = spawnPoint;
                obj.transform.rotation = Quaternion.Euler(UtilFunctions.RandomV3(angleOffset));
                obj.GetComponent<PipeObject>().PipeAnimation(true);
            }
            else
            {
                count++;
            }
        }
    }

}
