using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class PlayerEffect : MonoBehaviour
{
    private Controls control;

    [SerializeField] ParticleSystem flap;
    [SerializeField] ParticleSystem glide;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(control.jump))
        {
            flap.Play();
        }

        if (Input.GetKeyDown(control.dash))
        {
            ToggleGlide(true);
        }
        else if (Input.GetKeyUp(control.dash))
        {
            ToggleGlide(false);
        }
    }

    private void ToggleGlide(bool flag)
    {
        var main = glide.main;
        main.loop = flag;
        if (flag)
        {
            glide.Play();
        }
    }
}
