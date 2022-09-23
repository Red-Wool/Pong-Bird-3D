using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : PlatformTemplate
{
    private float lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Reset()
    {
        lifeTime = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerMove>().Ring();
        }
    }
}
