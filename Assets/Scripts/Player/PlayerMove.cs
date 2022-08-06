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

    [SerializeField] private PlayerEffect effect;

    [SerializeField] private Controls control;

    private Rigidbody rb;
    private AudioSource sfx;
    private SphereCollider hitBox;

    private bool isDash; public bool IsDash { get { return isDash; } }
    private Vector3 move; public Vector3 Move { get { return move; } }

    private float dashRaiseTime;
    private float jumpTime;

    private bool touching;
    private Vector3 input;
    private Vector3 dashDirection;
    private Vector3 contactPoint;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sfx = GetComponent<AudioSource>();
        hitBox = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(CheckGrounded());
        //HitBox
        //hitBox.center = Vector3.up * (touching ? stats.groundSize : stats.airSize);

        //Movement
        move = rb.velocity;
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (input.magnitude > 1f){input = input.normalized;}
        input = Quaternion.Euler(0f, Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + cam.eulerAngles.y, 0f) * Vector3.forward * stats.speed * input.magnitude;

        move.x = Mathf.Lerp(move.x, input.x, Time.deltaTime * stats.speedDecay);// + Mathf.Lerp(move.x, 0f, Time.deltaTime * stats.speedDecay);
        move.z = Mathf.Lerp(move.z, input.z, Time.deltaTime * stats.speedDecay);// + Mathf.Lerp(move.z, 0f, Time.deltaTime * stats.speedDecay);

        if (!isDash && move.y < 0f) //More Gravity when moving down
        {
            rb.AddForce(Physics.gravity * stats.downGravityScale * Time.deltaTime, ForceMode.Acceleration);
        }

        jumpTime -= Time.deltaTime;
        if (Input.GetKeyDown(control.jump)) //Jumping
        {
            if (CheckGrounded() && jumpTime < 0f)
            {
                jumpTime = stats.jumpCooldown;
            }
            
            move.y = stats.jump;
            if (isDash)
            {
                //StopCoroutine("Dash");
                //isDash = false;
                move.y *= stats.dashCancelJumpScale;
                dashRaiseTime = stats.dashRaiseTime;
                //dashDirection.x *= 1.5f;
                //dashDirection.z *= 1.5f;
            }
        }
        else if (Input.GetKey(control.jump))
        {
            
            if (touching && !CheckGrounded() && jumpTime < 0f) //WallJump
            {
                Debug.Log("WallJump");
                effect.Flap.Play();
                Vector3 backDist = (transform.position - contactPoint).normalized * stats.wallClimbJump.x;
                move = new Vector3(backDist.x, stats.wallClimbJump.y, backDist.z);
            }

            if (move.y > stats.holdJumpCap) //Allow Player to jump higher by holding
            {
                move.y += stats.holdJump * Time.deltaTime;
            }

            if (isDash && dashRaiseTime > 0f) //Raise player when in dash
            {
                //Only consider speed that isn't going up
                Vector3 temp = dashDirection;
                temp.y = Mathf.Pow(Mathf.Abs(Mathf.Min(temp.y, 0f)), 1f) * stats.dashRaiseYPower;

                dashDirection.x = Mathf.Lerp(dashDirection.x, 0f, Time.deltaTime * stats.dashRaiseSpeedDecay);
                dashDirection.z = Mathf.Lerp(dashDirection.z, 0f, Time.deltaTime * stats.dashRaiseSpeedDecay);

                dashDirection.y += Time.deltaTime * (stats.dashRaiseSpeedPower * temp.magnitude + stats.dashRaiseBasePower);
                dashRaiseTime -= Time.deltaTime;

                if (dashRaiseTime < 0f)
                {
                    isDash = false;
                }
            }
        }
        else if (Input.GetKeyUp(control.jump)) 
        {
            //Remove raise boost
            dashRaiseTime = -1;
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

            if (tempXZ.magnitude > stats.dashMaxMoveCap)
            {
                dashDirection.x += input.x * stats.dashMaxMoveScale * Time.deltaTime;
                dashDirection.z += input.z * stats.dashMaxMoveScale * Time.deltaTime;
                tempXZ = new Vector2(dashDirection.x, dashDirection.z);
            }
            if (tempXZ.magnitude > stats.dashMaxSpeed) // 
            {

                tempXZ = tempXZ.normalized * stats.dashMaxSpeed;
                dashDirection.x = tempXZ.x;
                dashDirection.z = tempXZ.y;
            }

            move = dashDirection;
        }

        /*if (UtilFunctions.Magnitude2D(move.x, move.z) > stats.maxSpeed)
        {
            Vector2 tempXZ = new Vector2(move.x, move.z).normalized * stats.dashMaxSpeed;
            move = new Vector3(tempXZ.x, move.y, tempXZ.y);
        }*/

        rb.velocity = move;
        rb.AddForce(Physics.gravity * stats.gravityScale * Time.deltaTime, ForceMode.Acceleration);
    }

    public void Pong()
    {
        //Debug.Log(move);
        move.x *= -1;
        move.z *= -1;
        //Debug.Log(move);
        rb.velocity = move;

        dashDirection.x *= -1;
        dashDirection.z *= -1;
    }

    public bool CheckGrounded()
    {
        return Physics.CheckBox(transform.position + stats.groundCheckCenter, stats.groundCheckSize, Quaternion.identity);
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
        if (collision.gameObject.tag != "Paddle")
        {
            isDash = false;
        }
        else
        {
            Pong();
        }
            
    }

    private void OnCollisionStay(Collision collision)
    {
        touching = true;
        contactPoint = collision.GetContact(0).point;
    }

    private void OnCollisionExit(Collision collision)
    {
        touching = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Paddle")
        {
            Pong();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + stats.groundCheckCenter, stats.groundCheckSize);
    }
}
