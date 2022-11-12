using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractObject : MonoBehaviour
{
    public float height;

    public abstract void Interact(PlayerMove player);
}
