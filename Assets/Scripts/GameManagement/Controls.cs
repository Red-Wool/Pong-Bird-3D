using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Controls", fileName = "New Controls")]
public class Controls : ScriptableObject
{
    public float sens;
    public KeyCode jump;
    public KeyCode left;
    public KeyCode right;
    public KeyCode dash;
}
