using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//Controls how the player moves
[RequireComponent(typeof(Rigidbody), typeof(AudioSource), typeof(PlayerRotate))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Transform cam;

    [SerializeField] private PlayerStats stats;

    private bool isDash; public bool IsDash { get { return isDash; } }

    [SerializeField] private Controls control;

    private Rigidbody rb;
    private AudioSource sfx;
    private Vector3 move; public Vector3 Move { get { return move; } }
    private float extraSpeed;

    private Vector3 input;
    private Vector3 dashDirection;

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
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (input.magnitude > 1f){input = input.normalized;}
        input = Quaternion.Euler(0f, Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + cam.eulerAngles.y, 0f) * Vector3.forward * stats.speed * input.magnitude;

        move.x = input.x;
        move.z = input.z;

        if (Input.GetKeyDown(control.jump)) //Jumping
        {
            move.y = stats.jump;
            if (isDash)
            {
                StopCoroutine("Dash");
                isDash = false;
                move.y *= stats.dashCancelJumpScale;
                //dashDirection.x *= 1.5f;
                //dashDirection.z *= 1.5f;
            }
        }
        if (!isDash && Input.GetKeyDown(control.dash)) //Initiate Dash Mode
        {
            StartCoroutine(Dash(stats.dashTime));
        }
        else if (isDash) //Dash Mode
        {
            if (!Input.GetKey(control.dash))
            {
                isDash = false;
            }
            dashDirection.y += stats.gravityScale * Time.deltaTime * -stats.dashGravity;
            dashDirection.x += input.x * stats.dashMoveScale * Time.deltaTime;
            dashDirection.z += input.z * stats.dashMoveScale * Time.deltaTime;

            Vector2 tempXZ = new Vector2(dashDirection.x, dashDirection.z);
            if (tempXZ.magnitude > stats.dashMaxSpeed)
            {
                tempXZ = tempXZ.normalized * stats.dashMaxSpeed;
                dashDirection.x = tempXZ.x;
                dashDirection.z = tempXZ.y;
            }

            move = dashDirection;
        }

        rb.velocity = move;
        rb.AddForce(Physics.gravity * stats.gravityScale * Time.deltaTime, ForceMode.Acceleration);
    }

    IEnumerator Dash(float time)
    {
        
        isDash = true;
        //extraSpeed = speed * dashPower;
        dashDirection = new Vector3(move.x, stats.dashVertVelScale * (move.y + 1f), move.z) * stats.dashPower;
        yield return new WaitForSeconds(time);

        if (!Input.GetKey(control.dash))
        {
            isDash = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isDash = false;
    }
}
