using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook cam;

    [SerializeField] private float startFov;
    [SerializeField] private AnimationCurve fovChange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PaddleHitAdjust(float angle)
    {
        cam.m_XAxis.Value = angle;
    }
}
