using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlBubble : MonoBehaviour
{
    [SerializeField] private PlayerMove player;

    [SerializeField] private AnimationCurve bubbleSmooth;
    [SerializeField] private float smoothSpeed;
    private float smoothTime;

    [SerializeField] private GameObject bubble;
    [SerializeField] private TMP_Text text;

    [SerializeField] private Controls control;

    private Camera cam;
    private InteractObject currentInter;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;

        if (currentInter)
        {
            smoothTime += Time.deltaTime * smoothSpeed;
            bubble.transform.position = cam.WorldToScreenPoint(currentInter.transform.position);
            if (Input.GetKeyDown(control.interact))
            {
                currentInter.Interact(player);
            }
        }
        else
        {
            smoothTime -= Time.deltaTime * smoothSpeed;
        }

        smoothTime = Mathf.Clamp01(smoothTime);
        bubble.transform.localScale = Vector3.one * bubbleSmooth.Evaluate(smoothTime);
    }

    private void OnTriggerStay(Collider other)
    {
        
        InteractObject inter = other.GetComponent<InteractObject>();
        if (inter && (currentInter == null || (other.transform.position - transform.position).magnitude < (currentInter.transform.position - transform.position).magnitude))
        {
            Debug.Log("Here");
            currentInter = inter;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentInter != null && currentInter.gameObject == other.gameObject)
        {
            currentInter = null;
        }   
    }
}
