using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

//Controls how the player moves
[RequireComponent(typeof(Rigidbody), typeof(AudioSource), typeof(PlayerRotate))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Vector3 respawnPoint;
    [SerializeField] private Transform cam;

    [SerializeField] private PlayerStats stats;

    [SerializeField] private PlayerEffect effect;

    [SerializeField] private Controls control;

    [SerializeField] private StoredValue hp;
    [SerializeField] private StoredValue jump;
    [SerializeField] private StoredValue stamina;
    [SerializeField] private StoredValue coin;

    private Rigidbody rb;
    private PlayerMusic music;
    private SphereCollider hitBox;

    private bool isDash; public bool IsDash { get { return isDash; } }
    private Vector3 move; public Vector3 Move { get { return move; } }

    private bool isDead;

    private bool isLate;
    private Vector3 lateMove;

    private float dashRaiseTime;
    private float jumpTime;
    private float noFeatherTime;
    private float wallTime;
    private float diveTime;
    private float invinTime;

    private bool touching;
    private bool grounded;
    private bool holdingJump;
    private Vector3 input;
    private Vector3 dashDirection;
    private Vector3 contactPoint;
    private GameObject touchObj;

    public static UnityEvent death;

    // Start is called before the first frame update
    void Start()
    {
        if (death == null)
        {
            death = new UnityEvent();
        }

        rb = GetComponent<Rigidbody>();
        music = GetComponent<PlayerMusic>();
        hitBox = GetComponent<SphereCollider>();

        hp.value = stats.maxHP;
        stamina.value = stats.maxStamina;
        jump.value = stats.maxJump;

        PaddleObject.PointScored += Grant;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = CheckGrounded();
        holdingJump = Input.GetKey(control.jump);

        if (grounded)
        {
            jump.value = stats.maxJump;
            ChangeStamina(Time.deltaTime * (isDash ? stats.staminaGrantGroundDashRegain : stats.staminaGroundRegain));
        }

        if (touching && !touchObj.activeSelf)
        {
            touching = false;
        }
        //Debug.Log(CheckGrounded());
        //HitBox
        //hitBox.center = Vector3.up * (touching ? stats.groundSize : stats.airSize);

        //Movement
        if (!isDead)
        {
            MovePlayer();
        }

        //Gravity
        rb.AddForce(Physics.gravity * stats.gravityScale * Time.deltaTime, ForceMode.Acceleration);
    }

    public void MovePlayer()
    {
        //Input
        move = rb.velocity;
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (input.magnitude > 1f) { input = input.normalized; }
        input = Quaternion.Euler(0f, Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + cam.eulerAngles.y, 0f) * Vector3.forward * stats.speed * input.magnitude;

        move.x = Mathf.Lerp(move.x, input.x, Time.deltaTime * stats.speedDecay);// + Mathf.Lerp(move.x, 0f, Time.deltaTime * stats.speedDecay);
        move.z = Mathf.Lerp(move.z, input.z, Time.deltaTime * stats.speedDecay);// + Mathf.Lerp(move.z, 0f, Time.deltaTime * stats.speedDecay);

        //More Gravity when moving down
        if (!isDash && move.y < 0f) 
        {
            rb.AddForce(Physics.gravity * stats.downGravityScale * Time.deltaTime, ForceMode.Acceleration);
        }

        //Timers
        jumpTime -= Time.deltaTime;
        noFeatherTime -= Time.deltaTime;
        invinTime -= Time.deltaTime;

        //Jumping
        if (Input.GetKeyDown(control.jump)) 
        {
            
            if (jump.value > 0)
            {
                wallTime = -1f;

                if (grounded && jumpTime < 0f)
                {
                    music.JumpSound(0);
                    jumpTime = stats.jumpCooldown;

                    if (isDash)
                    {
                        isDash = false;
                        move += UtilFunctions.MagnitudeChange(dashDirection, stats.dashGroundJump.x) + Vector3.up * stats.dashGroundJump.y;
                    }
                }
                //sfx.Play();
                effect.Flap.Play();
                move.y = stats.jump + ((grounded ? stats.diveGroundJump.y : stats.diveAirJump.y) * diveTime);
                move += UtilFunctions.MagnitudeChange(input, (grounded ? stats.diveGroundJump.x : stats.diveAirJump.x) * diveTime);

                diveTime = 0f;
            }

            if (isDash)
            {
                //StopCoroutine("Dash");
                //isDash = false;
                //move.y *= stats.dashCancelJumpScale;
                dashRaiseTime = stats.dashRaiseTime;
                //dashDirection.x *= 1.5f;
                //dashDirection.z *= 1.5f;
                if (!grounded)
                {
                    ChangeStamina(-stats.staminaRaiseCost);
                }
            }
            else if (!grounded && !touching && wallTime < 0f)
            {
                if (jump.value > 0)
                {
                    music.JumpSound((int)jump.value);
                    jump.value--;
                    
                    ChangeStamina(stats.staminaFlapRegain);
                }
                else if (noFeatherTime < 0f)
                {
                    music.JumpSound(0);

                    //Last Hope Jump
                    effect.NoFeather.Play();

                    noFeatherTime = stats.noFeatherJumpCooldown;
                    move.y = stats.noFeatherJump;

                    move.x *= stats.noFeatherJumpSlow;
                    move.z *= stats.noFeatherJumpSlow;
                }

            }
        }
        else if (holdingJump) //Hold Jump
        {
            wallTime -= Time.deltaTime;
            if (touching && !grounded && jumpTime < 0f && wallTime < 0f) //WallJump
            {
                //jump.value = stats.maxJump;
                //Debug.Log("WallJump");

                move = WallJump(contactPoint);
                touching = false;
                /*wallTime = stats.wallClimpCooldown;

                effect.Flap.Play();
                Vector3 backDist = UtilFunctions.MagnitudeChange(transform.position - contactPoint, stats.wallClimbJump.x);
                move = new Vector3(backDist.x, stats.wallClimbJump.y, backDist.z);*/
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

                //Stop dash and give more Up momentum
                if (dashRaiseTime < 0f)
                {
                    isDash = false;

                    move.y += stats.dashRaiseJumpBoost;
                }

                ChangeStamina(-stats.staminaRaiseRate * Time.deltaTime);
            }
        }
        else if (Input.GetKeyUp(control.jump))
        {
            //Remove raise boost
            dashRaiseTime = -1;
        }

        if (!isDash && Input.GetKeyDown(control.dash) && stamina.value > 0f) //Initiate Dash Mode
        {
            DashStart(stats.dashTime);
        }
        else if (isDash) //Dash Mode
        {
            if (!grounded || !touching)
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
            if (tempXZ.magnitude > stats.dashMaxSpeed) // if Max speed, slow down
            {

                tempXZ = tempXZ.normalized * stats.dashMaxSpeed;
                dashDirection.x = tempXZ.x;
                dashDirection.z = tempXZ.y;
            }

            //No Dash if certain conditions met            

            isDash = Input.GetKey(control.dash);

            ChangeStamina(-Time.deltaTime);

            move = dashDirection;
        }

        if (Input.GetKey(control.dive))
        {
            move.y -= stats.divePower * Time.deltaTime;
            dashDirection.y -= stats.divePower * Time.deltaTime;
            diveTime = Mathf.Min(diveTime + Time.deltaTime, stats.diveTimeCap);
        }
        else if (Input.GetKeyUp(control.dive))
        {
            diveTime = 0f;
        }

        //Move the player

        if (isLate)
        {
            //Debug.Log("Add it up " + lateMove);

            move += lateMove;

            isLate = false;
            lateMove = Vector3.zero;
        }

        rb.velocity = move;
    }

    public void Heal()
    {
        hp.value = stats.maxHP;
    }

    public void Death()
    {
        hp.value = 0;
        if (!isDead) 
            StartCoroutine(DeathAnimation());
    }

    public IEnumerator DeathAnimation()
    {
        TimeManager.Instance.TwoStepPause(0, .3f, .4f, 2f);

        isDead = true;
        Vector3 temp = rb.velocity;
        temp *= 1.5f;
        temp.y = Mathf.Clamp(temp.y, 5f, 30f) + Random.Range(20f, 30f);
        rb.velocity = temp;
        yield return new WaitForSeconds(2f);
        death.Invoke();
        yield return new WaitForSeconds(2f);
        isDead = false;

        transform.position = respawnPoint;
        hp.value = stats.maxHP;
    }

    public void Damage()
    {
        if (invinTime < 0 && !isDead)
        {
            Debug.Log("Ow");
            hp.value--;
            if (hp.value <= 0)
            {
                Death();
            }
            else
            {
                TimeManager.Instance.MiniPause(.2f, .5f);
            }
            invinTime = stats.invinTime;
        }
        
        
    }
    public void Grant()
    {
        stamina.value += stats.staminaGrantRegain;
        jump.value = Mathf.Min(jump.value + 1, stats.maxJump);

        TimeManager.Instance.MiniPause(0, .25f);
    }

    public void Ring()
    {
        Debug.Log("Ring it up");
        DashStart(stats.dashRingTime);
    }

    public void TouchCloud()
    {
        ChangeStamina(stats.staminaTouchCloud);
    }

    public void ChangeStamina(float val)
    {
        stamina.value = Mathf.Min(stamina.value + val, stats.maxStamina);

        if (stamina.value < 0f)
        {
            stamina.value = 0f;
            move.y += stats.dashRaiseJumpBoost * dashRaiseTime;
            isDash = false;
        }
    }

    public void DashStart(float time)
    {
        ChangeStamina(-stats.staminaDashCost);

        StartCoroutine(Dash(time));
    }

    IEnumerator Dash(float time)
    {

        isDash = true;
        //extraSpeed = speed * dashPower;
        dashDirection = new Vector3(move.x, stats.dashVertVelScale * (move.y + 1f), move.z) * stats.dashPower + input * stats.dashInputPower;
        yield return new WaitForSeconds(time);

        if (!Input.GetKey(control.dash))
        {
            isDash = false;
        }
    }

    public Vector3 WallJump(Vector3 contact)
    {
        wallTime = stats.wallClimpCooldown;

        effect.Flap.Play();
        Vector3 backDist = UtilFunctions.MagnitudeChange(transform.position - contactPoint, stats.wallClimbJump.x) + 
            UtilFunctions.MagnitudeChange(move, UtilFunctions.Magnitude2D(move.x,move.z) * stats.wallClimbSpeedAdd);
        return new Vector3(backDist.x, stats.wallClimbJump.y, backDist.z);
    }

    public void Launch(Vector3 dir)
    {
        isLate = true;

        dashDirection += dir;
        lateMove += dir; 
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
        foreach(Collider col in Physics.OverlapBox(transform.position + stats.groundCheckCenter, stats.groundCheckSize))
        {
            if (col.tag == "Ground")
                return true;
        }

        return false;
    }



    public void GetCoin(int num)
    {
        coin.value += num;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Slippery")
        {
            if (collision.gameObject.tag == "Death")
            {
                Death();
                return;
            }

            if (collision.gameObject.tag != "Paddle")
            {
                /*if (isDash)
                {
                    isDash = false;

                    if (CheckGrounded())
                    {
                        Debug.Log("Landed! " + dashDirection);
                        move = UtilFunctions.MagnitudeChange(dashDirection, stats.dashGroundLand.x) + Vector3.up * stats.dashGroundLand.y;
                        Debug.Log(move);
                    }



                }*/
                if (!grounded)
                    isDash = false;
            }
            else
            {
                Pong();
                invinTime = 0.5f;


            }
        }
        
            
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag != "Slippery")
        {
            touching = true;
            contactPoint = collision.GetContact(0).point;
            touchObj = collision.gameObject;
        }
        

        /*if (collision.gameObject.tag == "Ground" && CheckGrounded())
        {
            stamina.value = Mathf.Min(stamina.value + Time.deltaTime * stats.staminaGroundRegain, stats.maxStamina);
        }*/
    }

    private void OnCollisionExit(Collision collision)
    {
        touching = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Paddle")
        {
            /*if (!isDash && holdingJump && !grounded)
            {
                Debug.Log("Off The Wall!");
                //Debug.Log(other.transform.position);
                
                lateMove = WallJump(-other.transform.position);//other.ClosestPoint(transform.position));
                isLate = true;
            }*/
            Pong();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(lateMove, stats.groundCheckSize);
    }
}
