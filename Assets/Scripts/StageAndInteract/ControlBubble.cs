using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlBubble : MonoBehaviour
{
    [SerializeField] private GameObject player;

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
            
            bubble.transform.position = cam.WorldToScreenPoint(currentInter.transform.position);
            if (Input.GetKeyDown(control.interact))
            {

            }
        }
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
        if (currentInter == other.gameObject)
        {
            currentInter = null;
        }   
    }
}
