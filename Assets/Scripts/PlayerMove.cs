using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//Controls how the player moves
[RequireComponent(typeof(Rigidbody), typeof(AudioSource), typeof(PlayerRotate))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Vector3 speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float dashPower;
    [SerializeField] private float dashTime;

    [SerializeField] private float gravityScale;
    private bool isDash; public bool IsDash { get { return isDash; } }

    [SerializeField] private Controls control;

    private Rigidbody rb;
    private AudioSource sfx;
    private Vector3 move; public Vector3 Move { get { return move; } }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sfx = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        move = rb.velocity;
        move.x = Input.GetAxis("Horizontal")*speed.x;
        move.z = Input.GetAxis("Vertical")*speed.z;

        if (Input.GetKeyDown(control.jump)) 
        {
            move.y = jumpPower;
        }
        if (!isDash && Input.GetKeyDown(control.dash))
        {
            StartCoroutine(Dash(dashTime));
        }
        else if (isDash)
        {
            move.x *= dashPower;
            move.z *= dashPower;
        }

        rb.velocity = move;
        rb.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
    }

    IEnumerator Dash(float time)
    {
        
        isDash = true;
        yield return new WaitForSeconds(time);
        isDash = false;
    }
}
