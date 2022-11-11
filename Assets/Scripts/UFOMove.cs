using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class UFOMove : MonoBehaviour
{
    [SerializeField] private ObjectPool coin;
    [SerializeField] private Vector2 lifeTimeRange;

    [Space(10), Header("Move")]
    [SerializeField] private Vector3 wanderRange;
    [SerializeField] private Vector3 displace;
    [SerializeField] private GameObject player;

    [SerializeField] private float wanderSpeed;
    [SerializeField] private float scatterSpeed;
    [SerializeField] private float detect;

    [SerializeField] private float spinSpeed;
    [SerializeField] private float spinAccel;

    [Space(10), Header("Coin")]
    [SerializeField] private float coinRate;
    [SerializeField] private float appearMod;

    private Rigidbody rb;

    private Vector3 wander;
    private float lifeTime;
    private float coinTime;

    private bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        PaddleObject.PointScored += CheckSpawn;

        rb = GetComponent<Rigidbody>();
        isActive = false;

        coin.AddObjects();
    }

    public void CheckSpawn()
    {
        if (!isActive)
        {
            StartCoroutine(DelaySpawn());
        }
    }

    public IEnumerator DelaySpawn()
    {
        yield return 0;

        if (GameManager.Score % appearMod == 4 || Random.Range(0f, 1f) < .1f)
        {
            Reset();
        }
    }

    public void Reset()
    {
        isActive = true;

        lifeTime = Random.Range(lifeTimeRange.x, lifeTimeRange.y);
        wander = UtilFunctions.RandomV3(wanderRange) + displace;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            return;
        }

        coinTime -= Time.deltaTime;

        if (coinTime < 0)
        {
            GameObject obj = coin.GetObject();
            obj.GetComponent<Coin>().Reset(transform.position);

            coinTime = coinRate;
        }

        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
        {
            isActive = false;
            transform.DOMoveY(100f, 1f).SetEase(Ease.InOutSine);
        }

        Vector3 dis = transform.position - player.transform.position;

        if (dis.magnitude < detect)
        {
            rb.angularVelocity = Vector3.up * spinAccel;


            dis.y *= 0.1f;
            rb.AddForce(Time.deltaTime * dis.normalized * scatterSpeed);
            wander = UtilFunctions.RandomV3(wanderRange) + displace;
        }
        else
        {
            rb.angularVelocity = Vector3.up * spinSpeed;

            dis = wander - transform.position;
            if (dis.magnitude < 5f)
            {
                wander = UtilFunctions.RandomV3(wanderRange) + displace;
            }
            else
            {
                rb.AddForce(Time.deltaTime * (dis - rb.velocity).normalized * wanderSpeed);
            }
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            transform.position += Vector3.up * 100;

            isActive = false;
            other.GetComponent<PlayerMove>().GetCoin(5);
        }
    }
}
