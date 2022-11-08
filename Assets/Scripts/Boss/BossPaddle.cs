using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPaddle : MonoBehaviour
{
    public bool isActive;
    private BossPaddleContainer container;

    public void Reset(BossPaddleContainer con)
    {
        container = con;
        isActive = true;
        gameObject.SetActive(true);
    }

    public void PaddleHit()
    {
        isActive = false;
        container.PaddleHit();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("BossOw");
        if (other.transform.tag == "Player" && isActive)
        {
            Vector3 dir = other.transform.position - container.transform.position;
            dir.y = 1f;
            Debug.Log("BossOw");
            other.GetComponent<PlayerMove>().InvincibleLaunch(dir.normalized * 150);
            PaddleHit();
        }
    }
}
