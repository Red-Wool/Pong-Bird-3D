using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(PlayerMove))]
public class PlayerRotate : MonoBehaviour
{
    private PlayerMove p;

    [SerializeField] private Vector3 moveMult;

    private Vector3 rotate;

    // Start is called before the first frame update
    void Start()
    {
        p = GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        rotate.z = p.Move.x * -5;
        rotate.x = p.Move.z * 5;

        rotate.x *= moveMult.x;
        rotate.z *= moveMult.z;

        transform.rotation = Quaternion.Euler(Vector3.Slerp(FixRotation(transform.rotation.eulerAngles), rotate, 0.2f));
        
    }

    private Vector3 FixRotation(Vector3 euler)
    {
        if (euler.x > 180) { euler.x -= 360f; }
        if (euler.y > 180) { euler.y -= 360f; }
        if (euler.z > 180) { euler.z -= 360f; }
        return euler;
    }
}
