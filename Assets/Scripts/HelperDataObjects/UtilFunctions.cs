using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class UtilFunctions
{
    public static Vector3 RandomV3(Vector3 range)
    {
        return new Vector3(Random.Range(-range.x, range.x), Random.Range(-range.y, range.y), Random.Range(-range.z, range.z));
    }

    public static float Magnitude2D(float x, float z)
    {
        return new Vector2(x, z).magnitude;
    }

    public static void GrowObject(GameObject obj)
    {
        obj.transform.localScale = Vector3.zero;
        obj.transform.DOScale(Vector3.one, 0.1f);
    }

    public static void PulseObject(GameObject obj, float time, float iOpacity, float fOpacity, int loops)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        sr.DOKill();
        //sr.DOFade(0.5f, 0.1f).OnComplete(() => sr.DOFade(0f, 0.1f));
        sr.DOFade(iOpacity, time * 0.5f).OnComplete(() => sr.DOFade(fOpacity, time * 0.5f)).SetLoops(loops);
    }

    public static Vector2 Vec3ToVec2(Vector3 pos)
    {
        return new Vector2(pos.x, pos.y);
    }
    public static Vector3 Vec2ToVec3(Vector2 pos)
    {
        return new Vector3(pos.x, pos.y);
    }

    public static Vector2 RandVec2Range(Vector2 range)
    {
        return new Vector2(Random.Range(-range.x, range.x), Random.Range(-range.y, range.y));
    }

    public static float AngleTowards(Vector3 pos, Vector3 target)
    {
        Vector3 dir = target - pos;
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    public static bool CheckTimer(ref float timer, float time)
    {
        if (timer > time)
        {
            timer -= time;
            return true;
        }
        return false;
    }
}
