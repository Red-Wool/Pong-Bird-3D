﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider))]
public class PaddleObject : MonoBehaviour
{
    public delegate void Trigger();
    public static event Trigger PointScored;

    [SerializeField] private Vector3 size;
    [SerializeField] private float angleOffset;
    private float currentAngle;

    [SerializeField] private float distFromCenter;
    [SerializeField] private Vector2 yRange;

    private bool hit;

    private BoxCollider hitbox;

    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<BoxCollider>();
        currentAngle = 0;
        hit = false;
    }

    private void MoveAcross()
    {
        //Debug.Log(Random.Range(-angleOffset, angleOffset));
        currentAngle += 180f + Random.Range(-angleOffset, angleOffset);
        float angle = currentAngle * Mathf.Deg2Rad;

        transform.position = new Vector3(Mathf.Cos(angle) * distFromCenter, Random.Range(yRange.x, yRange.y), Mathf.Sin(angle) * distFromCenter);
        transform.LookAt(Vector3.up * transform.position.y);
    }

    /*private void PointScore()
    {
        PointScored.Invoke();
        MoveAcross();
    }*/

    public IEnumerator MoveAnimation()
    {
        hit = true;
        PointScored.Invoke();

        yield return new WaitForSeconds(.05f);

        transform.DOScale(Vector3.zero, .2f);

        yield return new WaitForSeconds(.2f);
        MoveAcross();

        transform.DOScale(size, .2f);
        hit = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hit) { return; }
        if (collision.transform.tag == "Player")
        {
            StartCoroutine(MoveAnimation());
        }
    }
}
