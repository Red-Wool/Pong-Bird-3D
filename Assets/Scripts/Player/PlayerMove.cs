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
    [SerializeField] private PlayerDisplay display;

    [SerializeField] private Controls control;

    [SerializeField] private StoredValue hp;
    [SerializeField] private StoredValue jump;
    [SerializeField] private StoredValue stamina;
    [SerializeField] private StoredValue coin;

    private Rigidbody rb;
    private PlayerMusic music;

    private bool isDash; public bool IsDash { get { return isDash; } }
    private bool isDive;
    private Vector3 move; public Vector3 Move { get { return move; } }

    private bool canMove;

    private bool isLate;
    private Vector3 lateMove;

    private float dashRaiseTime;
    private float jumpTime;
    private float noFeatherTime;
    private float noSpeedDecayTime;
    private float wallTime;
    private float pingPongTime;
    private float diveTime;
    private float diveBuffer;
    private float invinTime;

    private bool touching;
    private bool grounded;


    private bool canPingPong;
    private int pingPongStreak;
    private Vector3 curPingPongAngle;

    private bool hasStamina; public bool HasStamina { get { return hasStamina; } }
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

        canMove = true;

        hp.value = stats.maxHP;
        stamina.value = stats.maxStamina;
        jump.value = stats.maxJump;

        PaddleObject.PointScored += Grant;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = CheckGrounded();
        hasStamina = stamina.value > 0f;
        holdingJump = Input.GetKey(control.jump);
        isDive = Input.GetKey(control.dive);

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
        if (canMove)
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
        input = Quaternion.Euler(0f, Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + cam.eulerAngles.y, 0f) * Vector3.forward * 
            (stats.speed + (pingPongTime > stats.pingPongSpeedTime ? 1f : 0f) * pingPongStreak * stats.pingPongSpeedStrength) * input.magnitude;

        if (noSpeedDecayTime <= 0f)
        {
            move.x = Mathf.Lerp(move.x, input.x, Time.deltaTime * stats.speedDecay);// + Mathf.Lerp(move.x, 0f, Time.deltaTime * stats.speedDecay);
            move.z = Mathf.Lerp(move.z, input.z, Time.deltaTime * stats.speedDecay);// + Mathf.Lerp(move.z, 0f, Time.deltaTime * stats.speedDecay);
        }
        

        //More Gravity when moving down
        if (!isDash && move.y < 0f) 
        {
            rb.AddForce(Physics.gravity * stats.downGravityScale * Time.deltaTime, ForceMode.Acceleration);
        }

        //Timers
        jumpTime -= Time.deltaTime;
        noFeatherTime -= Time.deltaTime;
        invinTime -= Time.deltaTime;
        noSpeedDecayTime -= Time.deltaTime;
        
        //Ping Pong Action
        if (canPingPong)
        {
            pingPongTime -= Time.deltaTime;

            if (pingPongTime <= 0f)
            {
                canPingPong = false;
                pingPongStreak = 0;
            }
        }

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
                move.y = stats.jump + ((grounded ? stats.diveGroundJump.y : stats.diveAirJump.y) * Mathf.Min(diveTime, stats.diveTimeCap));
                move += UtilFunctions.MagnitudeChange(input, (grounded ? stats.diveGroundJump.x : stats.diveAirJump.x) * Mathf.Min(diveTime, stats.diveTimeCap));

                //diveTime = 0f;
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
                move = WallJump(contactPoint);
                touching = false;
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

                dashDirection.y += Time.deltaTime * (stats.dashRaiseSpeedPower * temp.magnitude + stats.dashRaiseBasePower) * (hasStamina ? 1f : stats.dashRaiseNoStaminaScale);
                dashRaiseTime -= Time.deltaTime;

                //Stop dash and give more Up momentum
                if (dashRaiseTime < 0f)
                {
                    isDash = false;

                    if (hasStamina)
                    {
                        move.y += stats.dashRaiseJumpBoost;
                    }
                }

                ChangeStamina(-stats.staminaRaiseRate * Time.deltaTime);
            }
        }
        else if (Input.GetKeyUp(control.jump))
        {
            //Remove raise boost
            dashRaiseTime = -1;
        }

        if (!isDash && Input.GetKeyDown(control.dash)) //Initiate Dash Mode
        {
            DashStart(stats.dashTime);
        }
        else if (isDash) //Dash Mode
        {
            if (!grounded || !touching)
                dashDirection.y -= stats.gravityScale * Time.deltaTime * (hasStamina ? stats.dashGravity : stats.dashNoStaminaGravity);
            if (grounded)
                dashDirection.y = Mathf.Max(dashDirection.y, -stats.diveMaxDownGroundSpd);

            dashDirection.x += input.x * (hasStamina ? stats.dashMoveScale : stats.dashMoveNoStaminaScale) * Time.deltaTime;
            dashDirection.z += input.z * (hasStamina ? stats.dashMoveScale : stats.dashMoveNoStaminaScale) * Time.deltaTime;

            Vector2 tempXZ = new Vector2(dashDirection.x, dashDirection.z);


            if (tempXZ.magnitude > stats.dashMaxMoveCap && hasStamina)
            {
                dashDirection.x += input.x * stats.dashMaxMoveScale * Time.deltaTime;
                dashDirection.z += input.z * stats.dashMaxMoveScale * Time.deltaTime;
                tempXZ = new Vector2(dashDirection.x, dashDirection.z);
            }
            if (tempXZ.magnitude > stats.dashMaxSpeed) // if Max speed, slow down
            {

                tempXZ = tempXZ.normalized * Mathf.Lerp(tempXZ.magnitude, stats.dashMaxSpeed, Time.deltaTime * stats.dashMaxDecay);
                dashDirection.x = tempXZ.x;
                dashDirection.z = tempXZ.y;
            }

            if (isDive)
            {
                dashDirection.y -= stats.dashDivePower * Time.deltaTime;
                Vector3 temp = new Vector3(dashDirection.x, 0, dashDirection.z).normalized;
                dashDirection += temp * stats.dashDiveSpeedIncrease * Time.deltaTime;
            }

            //No Dash if certain conditions met            

            isDash = Input.GetKey(control.dash);

            ChangeStamina(-Time.deltaTime);

            move = dashDirection;
        }

        if (isDive)
        {
            move.y -= stats.divePower * Time.deltaTime;
            
            diveTime += Time.deltaTime;
        }
        else if (Input.GetKeyUp(control.dive))
        {
            diveBuffer = stats.diveBuffer;
        }
        else if (!isDive && diveBuffer > 0f)
        {
            diveBuffer -= Time.deltaTime;
            if (diveBuffer < 0f)
            {
                diveTime = 0f;
            }
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
        if (canMove) 
            StartCoroutine(DeathAnimation());
    }

    public IEnumerator DeathAnimation()
    {
        TimeManager.Instance.TwoStepPause(0, .3f, .4f, 2f);

        canMove = false;
        Vector3 temp = rb.velocity;
        temp *= 1.5f;
        temp.y = Mathf.Clamp(temp.y, 5f, 30f) + Random.Range(20f, 30f);
        rb.velocity = temp;
        yield return new WaitForSeconds(2f);
        death.Invoke();
        yield return new WaitForSeconds(2f);
        canMove = true;

        transform.position = respawnPoint;
        hp.value = stats.maxHP;
    }

    public void Stop()
    {
        isDash = false;
        rb.velocity = rb.velocity.normalized * Mathf.Min(rb.velocity.magnitude,.5f);
        canMove = false;

        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        canMove = true;
    }

    public void Damage()
    {
        if (invinTime < 0 && canMove)
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

        move *= stats.ringSpeedMult;
        dashDirection *= stats.ringSpeedMult;
        ChangeStamina(stats.ringStaminaRegain);
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
            //move.y += stats.dashRaiseJumpBoost * dashRaiseTime;
            //isDash = false;
        }
    }

    public void DashStart(float time)
    {
        

        StartCoroutine(Dash(time));
    }

    IEnumerator Dash(float time)
    {

        isDash = true;
        Vector3 xz = new Vector3(move.x, 0, move.z);
        //extraSpeed = speed * dashPower;
        if (hasStamina)
        {
            Vector3 i = (xz.normalized - input.normalized) * Mathf.Min(xz.magnitude, 1f) * Mathf.Min(input.magnitude, 1f);
            //Debug.Log(xz + " " + input);
            //i *= 1 - Mathf.Max(i.magnitude - stats.dashInputDirDecay, 0f);
            dashDirection = UtilFunctions.MagnitudeChange(xz + input * stats.dashInputDir,
                UtilFunctions.Magnitude2D(move.x, move.z) * stats.dashPower * (1 - Mathf.Max(i.magnitude - stats.dashInputDirDecay, 0f))) + 
                Vector3.up * stats.dashVertVelScale * (move.y + 1f) * stats.dashPower;
        }
            //new Vector3(move.x, stats.dashVertVelScale * (move.y + 1f), move.z) * stats.dashPower + input * stats.dashInputPower;
        else
            dashDirection = xz + Vector3.up * (move.y > 0f ? move.y * stats.dashVertVelScale : move.y);// new Vector3(move.x, (move.y > 0f ? move.y * stats.dashVertVelScale : move.y), move.z);

        ChangeStamina(-stats.staminaDashCost);
        yield return new WaitForSeconds(time);

        if (!Input.GetKey(control.dash))
        {
            isDash = false;
        }
    }

    public Vector3 WallJump(Vector3 contact)
    {
        wallTime = stats.wallClimpCooldown;

        Vector3 backDist = transform.position - contact;
        backDist.y = 0;
        backDist = backDist.normalized;

        //Debug.Log(pingPongStreak + " Streak " + Vector3.Angle(curPingPongAngle, backDist));
        if (pingPongTime > 0f && Mathf.Abs(Vector3.Angle(curPingPongAngle, backDist)) >= stats.pingPongAngleStreakLimit)
        {
            Debug.Log("Increase");
            canPingPong = true;
            pingPongStreak = Mathf.Min(pingPongStreak+1, stats.pingPongMaxStreak);
        }
        curPingPongAngle = backDist;
        

        effect.Flap.Play();
        backDist = UtilFunctions.MagnitudeChange(transform.position - contactPoint, stats.wallClimbJump.x + (diveTime > stats.diveWallChargeTime ? stats.diveWallJump.x : 0) + 
            ((canPingPong ? 1f : 0f) * pingPongStreak * stats.pingPongWallJumpStrength) ) + 
            UtilFunctions.MagnitudeChange(move.normalized + (canPingPong ? 1f : 0f) * input * stats.pingPongAngleStrength, 
            UtilFunctions.Magnitude2D(move.x,move.z) * stats.wallClimbSpeedAdd);

        /*if (canPingPong)
        {
            backDist = UtilFunctions.MagnitudeChange(backDist.normalized + input * stats.pingPongAngleStrength, backDist.magnitude + pingPongStreak * stats.pingPongSpeedStrength);
        }*/

        backDist.y = stats.wallClimbJump.y + (diveTime > stats.diveWallChargeTime ? stats.diveWallJump.y : 0);


        pingPongTime = stats.pingPongTime;
        diveTime = 0f;

        return backDist;

        
    }

    public void InvincibleLaunch(Vector3 dir)
    {
        invinTime = 1f;
        jump.value = stats.maxJump;
        stamina.value = stats.maxStamina;

        dir.y += 1f + (move.y > 0f ? move.y * .5f : 0);

        noSpeedDecayTime = stats.bossPaddleNoSpeedDecayTime;
        TimeManager.Instance.MiniPause(0f, .25f);
        Launch(dir);
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
        if (display != null)
        {
            display.CoinGet(num);
        }
        else
            Debug.Log("NoDispaly");
        
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
                invinTime = 1f;


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
