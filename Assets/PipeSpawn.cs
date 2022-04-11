using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawn : MonoBehaviour
{
    public Transform Pipespawnprefab;
    public int objectcount = 10;
    public float spawnradius = 30;
    public float spawnCollisionCheck;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int loop = 0; loop < objectcount; loop++)
        {
            Vector3 spawnPoint = transform.position + Random.insideUnitSphere * spawnradius;

            if (!Physics.CheckSphere(spawnPoint, spawnCollisionCheck))
            {
                Instantiate(Pipespawnprefab, spawnPoint, Random.rotation);
            }
        }
    }

}
