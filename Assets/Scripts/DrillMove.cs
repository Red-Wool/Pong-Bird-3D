using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class DrillMove : EnemyTemplate
{
    [SerializeField] private float speed;

    [SerializeField] private bool isHoming;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void Reset(Vector3 pos, GameObject t)
    {
        base.Reset(pos, t);

        Debug.Log(Target.transform.position);
        transform.LookAt(Target.transform.position + UtilFunctions.RandomV3(new Vector3(25f, 15f, 25f)) + Vector3.up * 12f);
        rb.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * speed;
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void Spawn()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Death")
        {
            gameObject.SetActive(false);
        }
    }
}
