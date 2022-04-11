using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(PlayerMove))]
public class PlayerRotate : MonoBehaviour
{
    private PlayerMove p;

    [SerializeField] private AnimationCurve rotateMult;

    private Vector3 rotate;

    // Start is called before the first frame update
    void Start()
    {
        p = GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rotate.x = p.Move.magnitude * rotateMult.Evaluate(p.Move.magnitude);
        //rotate.z = p.Move.z * moveMult.z * -1;
        

        //rotate.y = Mathf.Atan2(rotate.x, rotate.z) * Mathf.Rad2Deg;

        //rotate.x *= moveMult.x;
        //rotate.z *= moveMult.z;

        rotate.y = Mathf.Atan2(p.Move.x, p.Move.z) * Mathf.Rad2Deg * p.Move.normalized.magnitude;

        transform.rotation = Quaternion.Euler(Vector3.Slerp(FixRotation(transform.rotation.eulerAngles), rotate, 0.1f));
        
    }

    private Vector3 FixRotation(Vector3 euler)
    {
        if (euler.x > 180) { euler.x -= 360f; }
        if (euler.y > 180) { euler.y -= 360f; }
        if (euler.z > 180) { euler.z -= 360f; }
        return euler;
    }
}
