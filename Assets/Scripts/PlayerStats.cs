using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stats", menuName = "Gameplay/Player Stats")]
public class PlayerStats : ScriptableObject
{
    public float speed;
    public float jump;
    public float gravityScale;

    public float dashPower;
    public float dashTime;
    public float dashGravity;
    public float dashMoveScale;
    public float dashMaxSpeed;
    public float dashVertVelScale;
    public float dashCancelJumpScale;
    
}
