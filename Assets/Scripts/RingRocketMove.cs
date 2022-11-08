using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RingRocketMove : MonoBehaviour
{
    [SerializeField] private EnemyBulletAttack ringGet;

    [SerializeField] private ObjectPool ring;
    [SerializeField] private GameObject player;
    [SerializeField] private float detect;
    [SerializeField] private bool send;

    [SerializeField] private float wanderSpeed;
    [SerializeField] private float scatterSpeed;

    [SerializeField] private float ringRate;
    [SerializeField] private Vector2 lifeTimeRange;
    [SerializeField] private Vector2 changeDirRate;
    [SerializeField] private float dirChange;
    [SerializeField] private float yChange;

    private Rigidbody rb;
    private bool isActive;

    private Vector3 dir;

    private float ringTime;
    private float lifeTime;
    private float changeTime;

    private float yLevel;

    // Start is called before the first frame update
    void Start()
    {
        PaddleObject.PointScored += Reset;

        ring.AddObjects();
        ringGet.Reset();

        rb = GetComponent<Rigidbody>();
    }

    public void Reset()
    {
        if (!isActive && Random.Range(0, 1f) < .5f)
        {
            isActive = true;

            Vector3 origin = Random.insideUnitCircle.normalized;

            lifeTime = Random.Range(lifeTimeRange.x, lifeTimeRange.y);
            changeTime = 3f;

            transform.transform.position = new Vector3(origin.x, 0, origin.y) * 1000f;

            transform.localScale = Vector3.zero;
            transform.DOScale(1f, 1f);

            transform.DOMove(new Vector3(origin.x, 0, origin.y) * 200f, 2f);

            dir = -new Vector3(origin.x, 0, origin.y);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (send)
        {
            send = false;
            Reset();
        }
        if (!isActive)
        {
            return;
        }

        transform.rotation = Quaternion.LookRotation(dir);
        ringTime -= Time.deltaTime;

        if (ringTime < 0)
        {
            GameObject obj = ring.GetObject();
            obj.transform.position = transform.position;
            obj.transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));

            obj.GetComponent<Ring>().Reset();

            Debug.Log("Ring it up");
            ringTime = ringRate;
        }

        lifeTime -= Time.deltaTime;

        Vector3 dis = transform.position - player.transform.position;

        if (lifeTime < 0)
        {
            isActive = false;
            dir.y *= .01f;
            dis = transform.position.normalized;
            dis.y = 0;

            transform.DOMove(dis * 800f, 1f).SetEase(Ease.InOutSine);
            transform.DOScale(0, 5f);

            return;
        }

        if (dis.magnitude < detect)
        {

            dis.y *= 0.1f;
            rb.AddForce(Time.deltaTime * dis.normalized * scatterSpeed);
        }
        else
        {
            changeTime -= Time.deltaTime;
            if (changeTime < 0f)
            {
                float a = Random.Range(-dirChange,dirChange) * Mathf.Deg2Rad;
                Vector3 center = transform.position.normalized * .4f;
                center.y = 0;

                dir = (new Vector3(Mathf.Cos(a) * dir.x - Mathf.Sin(a) * dir.z, 0, Mathf.Sin(a) * dir.x + Mathf.Cos(a) * dir.z) - center).normalized;

                yLevel = Random.Range(-5f, 10f);

                changeTime = Random.Range(changeDirRate.x, changeDirRate.y);
            }
            else
            {
                rb.AddForce(Time.deltaTime * dir * wanderSpeed);

                dir.y = Mathf.Lerp(dir.y, Mathf.Clamp(-(transform.position.y-yLevel),-2,2), Time.deltaTime * yChange);
            }
        }
    }
}
