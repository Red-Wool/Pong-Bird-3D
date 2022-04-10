using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerArrow : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject follow;
    [SerializeField] private GameObject target;

    [SerializeField] private Vector3 offset;
    [SerializeField] private float rotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        UpdateTracker();
    }

    public void UpdateTracker()
    {
        arrow.transform.position = follow.transform.position + offset;
        arrow.transform.rotation = Quaternion.RotateTowards(arrow.transform.rotation, Quaternion.LookRotation(target.transform.position - arrow.transform.position), rotateSpeed);
    }

}
