using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gameplay/Drill Type")]
public class DrillStats : ScriptableObject
{
    public float speed;
    public float detonTime;
    public bool isHoming;
}
