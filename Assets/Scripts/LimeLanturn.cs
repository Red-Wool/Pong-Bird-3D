using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LimeLanturn : EnemyTemplate
{
    [SerializeField] private EnemyBulletAttack warn;
    [SerializeField] private EnemyBulletAttack laser;
    [SerializeField] private EnemyBulletAttack laserOut;
    [SerializeField] private EnemyBulletAttack final;

    [SerializeField] private float shootTime;

    private float ammo;
    private float timer;

    public override void Reset(Vector3 pos, GameObject t)
    {
        base.Reset(pos, t);
        timer = 0;
        ammo = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (shootTime < timer)
        {
            timer = 0f;
            if (ammo > 0)
            {
                StartCoroutine(Laser());
            }
            else
            {
                StartCoroutine(Finish());
            }
            ammo -= 1;

        }
    }

    public IEnumerator Laser()
    {
        warn.SpawnAttack(transform.position, Quaternion.identity);
        yield return new WaitForSeconds(4f);
        laser.SpawnAttack(transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        laserOut.SpawnAttack(transform.position + Vector3.up * (Target.transform.position.y - transform.position.y), Quaternion.identity);
    }

    public IEnumerator Finish()
    {
        transform.DOMoveY(Target.transform.position.y, 5f).SetEase(Ease.InOutSine);

        yield return new WaitForSeconds(7f);

        final.SpawnAttack(transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
