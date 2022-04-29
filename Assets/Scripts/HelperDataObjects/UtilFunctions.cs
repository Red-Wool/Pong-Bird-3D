using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilFunctions
{
    public static Vector3 RandomV3(Vector3 range)
    {
        return new Vector3(Random.Range(-range.x, range.x), Random.Range(-range.y, range.y), Random.Range(-range.z, range.z));
    }
}
