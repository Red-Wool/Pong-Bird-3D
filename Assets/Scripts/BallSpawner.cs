using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject prefab;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", 0f, time);
    }

    void Spawn()
    {
        Instantiate(prefab, transform);
    }
}
