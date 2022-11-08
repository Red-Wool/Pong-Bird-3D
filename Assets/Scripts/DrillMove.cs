using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class DrillMove : EnemyTemplate
{
    [SerializeField] private DrillStats stat;
    [SerializeField] private EnemyBulletAttack shoot;

    private bool explode;
    private bool canGrant;
    private float timer;
    private Rigidbody rb;

    private float shootTime;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void Reset(Vector3 pos, GameObject t)
    {
        base.Reset(pos, t);

        //Debug.Log(Target.transform.position);
        transform.LookAt(Target.transform.position + UtilFunctions.RandomV3(new Vector3(25f, 5f, 25f)) + Vector3.up * 1f);
        rb.velocity = transform.forward * stat.speed;
        explode = false;
        canGrant = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (explode)
        {
            timer -= Time.deltaTime;
            if (timer < 0f)
            {
                shoot.SpawnAttack(transform.position, transform.rotation);
                gameObject.SetActive(false);
            }
        }

        if (stat.isHoming)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Target.transform.position - transform.position), Time.deltaTime * stat.homeStrength);
        }

        if (stat.isShoot)
        {
            shootTime -= Time.deltaTime;

            if (shootTime < 0f)
            {
                stat.shootAttack.SpawnAttack(transform.position, transform.rotation);
                shootTime = stat.shootRate;
            }
        }

        //transform.Rotate(transform.forward, stat.spin * Time.time);

        rb.velocity = transform.forward * stat.speed;
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void Spawn()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!explode)
        {
            explode = true;
            timer = stat.detonTime;
        }
        if (canGrant && collision.transform.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMove>().Grant();
            canGrant = false;
        }
        /*if (collision.transform.tag != "Enemy")
        {

        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Death")
        {
            gameObject.SetActive(false);
        }
    }
}
