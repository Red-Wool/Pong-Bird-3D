using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EnemyBullet : MonoBehaviour
{
    [HideInInspector] public ParticleSystem ps;

    private List<ParticleCollisionEvent> eventList = new List<ParticleCollisionEvent>();

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerMove>().Damage();
        }
        //int event
    }
}
