using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerArrow : MonoBehaviour
{
    [SerializeField] private Controls control;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject follow;
    [SerializeField] private GameObject target;

    [SerializeField] private Vector3 offset;
    [SerializeField] private float rotateSpeed;

    [SerializeField] private AnimationCurve toggleSize;
    [SerializeField] private float timeActive;
    private float timer;
    private bool isOn;

    // Start is called before the first frame update
    void Start()
    {
        isOn = false;
        PaddleObject.PointScored += ActivateArrow;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTracker();

        timer = Mathf.Clamp(timer + ((isOn ? 1 : -1) * Time.deltaTime), 0f, 1f);
        arrow.transform.localScale = Vector3.one * toggleSize.Evaluate(timer);

        if (Input.GetKeyDown(control.interact))
        {
            ActivateArrow();
        }
    }

    public void UpdateTracker()
    {
        arrow.transform.position = follow.transform.position + offset;
        arrow.transform.rotation = Quaternion.RotateTowards(arrow.transform.rotation, Quaternion.LookRotation(target.transform.position - arrow.transform.position), rotateSpeed);
    }

    public void ActivateArrow()
    {
        StopCoroutine("ActivateForTime");
        StartCoroutine(ActivateForTime(timeActive));
    }

    public IEnumerator ActivateForTime(float time)
    {
        isOn = true;
        yield return new WaitForSeconds(time);
        isOn = false;

    }

}
