using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private bool hasPlatform; public bool HasPlatform { get { return hasPlatform; } }
    [SerializeField] private GameObject spawnIsland;

    [SerializeField] private PlatformSpawnTable prefab;
    [SerializeField] private PlatformLocation[] platform;

    private void Awake()
    {
        foreach(PlatformSpawn p in prefab.table)
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
            if (p.HasPlatform)
            {
                if (Random.Range(0f, 1f) < .1f)
                {
                    p.Spawn(prefab.table[Random.Range(0, prefab.table.Length)]);
                }
                else if (Random.Range(0f, 1f) < .5f)
                {
                    p.Remove();
                }
            }
            else if (Random.Range(0f, 1f) < .2f)
            {
                p.Spawn(prefab.table[Random.Range(0, prefab.table.Length)]);
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
