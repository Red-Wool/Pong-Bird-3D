using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stats", menuName = "Gameplay/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Speed")]
    public float speed;
    public float maxSpeed;
    public float speedDecay;

    [Space(10),Header("Jump")]
    public float jump;
    public float jumpCooldown;

    public float holdJump;
    public float holdJumpCap;

    [Space(10), Header("Ground Check")] //hitBox
    public Vector3 groundCheckCenter;
    public Vector3 groundCheckSize;

    [Space(10), Header("Wall Climb")]
    public Vector2 wallClimbJump;

    [Space(10), Header("Gravity")]
    public float gravityScale;
    public float downGravityScale;

    [Space(10),Header("Dash")]
    public float dashPower;
    public float dashTime;

    [Space(10)]
    public float dashGravity;

    public float dashMoveScale;
    public float dashMaxMoveScale;

    [Space(10), Header("Dash Raise")]
    public float dashRaiseTime;
    public float dashRaiseSpeedPower;
    public float dashRaiseYPower;
    public float dashRaiseBasePower;
    public float dashRaiseSpeedYCap;
    [Space(10)]
    public float dashRaiseSpeedDecay;
    [Space(10)]

    public float dashMaxSpeed;
    public float dashMaxMoveCap;

    public float dashVertVelScale;
    public float dashCancelJumpScale;
    
}
