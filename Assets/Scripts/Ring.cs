using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ring : PlatformTemplate
{
    [SerializeField] private EnemyBulletAttack ringGet;

    private float lifeTime;

    private bool death;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Reset()
    {
        lifeTime = 10f;
        transform.DOScale(1, .5f);
        death = false;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0f )
        {
            if (death)
                gameObject.SetActive(false);
            else
                Death();
        }
    }

    void Death()
    {
        lifeTime = .5f;
        death = true;
        transform.DOScale(0, .5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !death)
        {
            other.GetComponent<PlayerMove>().Ring();
            ringGet.SpawnAttack(transform.position, transform.rotation);
            Death();
        }
    }
}
