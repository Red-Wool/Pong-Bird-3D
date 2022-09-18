using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    private Collider hitBox;
    private Rigidbody rb;

    private float lifetime;
    // Start is called before the first frame update
    void Start()
    {
        hitBox = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    public void Reset(Vector3 pos)
    {
        transform.position = pos;

        lifetime = 15f;

        hitBox = GetComponent<Collider>();
        hitBox.enabled = true;
        rb = GetComponent<Rigidbody>();
        rb.velocity = UtilFunctions.RandomV3(new Vector3(10, 5, 10) + Vector3.up * 15);

        transform.localScale = Vector3.one;
        transform.DOScale(1, 1f);

    }

    private void Update()
    {
        if (hitBox.enabled)
        {
            lifetime -= Time.deltaTime;
            if (lifetime < 0f)
            {
                StartCoroutine(Disappear());
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerMove>().GetCoin(1);
            StartCoroutine(Disappear());
        }
    }

    public IEnumerator Disappear()
    {
        hitBox.enabled = false;
        transform.DOScale(0, .1f);
        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }
}
