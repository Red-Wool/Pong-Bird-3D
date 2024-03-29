﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stats", menuName = "Gameplay/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Space(10), Header("Stat")]
    public float maxHP;
    public float maxJump;

    public float maxStamina;
    [Space(10)]
    public float invinTime;
    [Space(10)]
    public float staminaGroundRegain;
    public float staminaFlapRegain;
    public float staminaGrantRegain;
    public float staminaGrantGroundDashRegain;
    public float staminaTouchCloud;
    [Space(10)]
    public float staminaDashCost;
    public float staminaRaiseCost;
    public float staminaRaiseRate;


    [Space(10), Header("Speed")]
    public float speed;
    public float maxSpeed;
    public float speedDecay;

    [Space(10),Header("Jump")]
    public float jump;
    public float jumpCooldown;

    public float noFeatherJump;
    public float noFeatherJumpSlow;
    public float noFeatherJumpCooldown;

    public float holdJump;
    public float holdJumpCap;

    [Space(10), Header("Dive")]
    public float divePower;

    public float diveTimeCap;
    public Vector2 diveGroundJump;
    public Vector2 diveGroundDash;
    public Vector2 diveAirJump;

    public float diveBuffer;

    public float diveMaxDownGroundSpd;

    [Space(10), Header("Dive Wall Climb")]
    public Vector2 diveWallJump;
    public float diveWallChargeTime;

    [Space(10), Header("Ground Check")] //hitBox
    public Vector3 groundCheckCenter;
    public Vector3 groundCheckSize;

    [Space(10), Header("Wall Climb")]
    public Vector2 wallClimbJump;
    public float wallClimbSpeedAdd;
    public float wallClimpCooldown;

    [Space(10), Header("Ping Pong")]
    public float pingPongTime;
    public float pingPongAngleStreakLimit;

    public int pingPongMaxStreak;
    public float pingPongAngleStrength;
    public float pingPongSpeedStrength;
    public float pingPongDashConvert;

    [Space(10), Header("Gravity")]
    public float gravityScale;
    public float downGravityScale;

    [Space(10),Header("Dash")]
    public float dashPower;
    public float dashInputPower;
    public float dashTime;
    public float dashRingTime;

    [Space(10)]
    public float dashGravity;
    public float dashNoStaminaGravity;

    public float dashMoveScale;
    public float dashMoveNoStaminaScale;
    public float dashMaxMoveScale;

    [Space(10), Header("Dash Dive")]
    public float dashDivePower;
    public float dashDiveSpeedIncrease;

    [Space(10), Header("Dash Ground")]

    public Vector2 dashGroundLand;
    public Vector2 dashGroundJump;

    [Space(10), Header("Dash Raise")]
    public float dashRaiseTime;
    public float dashRaiseSpeedPower;
    public float dashRaiseYPower;
    public float dashRaiseBasePower;
    public float dashRaiseSpeedYCap;
    [Space(10)]
    public float dashRaiseSpeedDecay;
    public float dashRaiseJumpBoost;

    public float dashRaiseNoStaminaScale;
    [Space(10)]

    public float dashMaxSpeed;
    public float dashMaxMoveCap;
    public float dashMaxDecay;

    public float dashVertVelScale;
    public float dashCancelJumpScale;

    [Space(10), Header("Ring Effects")]
    public float ringStaminaRegain;
    public float ringSpeedMult;
    public float ringNoSpeedDecayTime;

    [Space(10), Header("Other")]
    public float bossPaddleNoSpeedDecayTime;

    [Space(10), Header("Time Effects")]
    public float grantStopTime;

}
