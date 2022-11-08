using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    [SerializeField] private float power;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            PlayerMove player = collision.gameObject.GetComponent<PlayerMove>();
            //if (player.IsDash)
            //{
                //player.Pong();
                //return;
            //}

            //Debug.Log("Boing");
            Vector3 launch = ((collision.transform.position) - collision.GetContact(0).point).normalized * power;
            if (launch.y > -8f && launch.y < 8f)
            {
                launch.y += power * .8f;
            }

            player.Launch(launch);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            PlayerMove player = collision.gameObject.GetComponent<PlayerMove>();
            if (player.IsDash)
            {
                Vector3 launch = ((collision.transform.position) - collision.GetContact(0).point).normalized * power;
                collision.gameObject.GetComponent<PlayerMove>().Launch(launch);
            }
        }
    }
}
