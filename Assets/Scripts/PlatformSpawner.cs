using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnIsland;

    [SerializeField] private PlatformSpawn[] prefab;
    [SerializeField] private PlatformLocation[] platform;

    private void Awake()
    {
        foreach(PlatformSpawn p in prefab)
        {
            p.prefab.AddObjects();
        }

        ConfigurePlatforms();
        PaddleObject.PointScored += ConfigurePlatforms;
    }

    public void ConfigurePlatforms()
    {
        foreach (PlatformLocation p in platform)
        {
            if (Random.Range(0f,1f) < .3f)
            {
                p.Spawn(prefab[Random.Range(0, prefab.Length)]);
            }
            else
            {
                p.Remove();
            }
        }
    }
}

[System.Serializable]
public struct PlatformSpawn
{
    public ObjectPool prefab;
    public Vector3 randomSpawnRange;
}
