using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class PlayerEffect : MonoBehaviour
{
    private PlayerMove player;
    [SerializeField] private Controls control;

    [SerializeField] ParticleSystem flap;
    [SerializeField] ParticleSystem glide;
    [SerializeField] ParticleSystem line;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMove>();
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
        else if (Input.GetKeyUp(control.dash) || !player.IsDash)
        {
            ToggleGlide(false);
        }
    }

    private void ToggleGlide(bool flag)
    {
        var main = glide.main;
        main.loop = flag;
        var lineMain = line.main;
        lineMain.loop = flag;
        
        if (flag)
        {
            glide.Play();
        }
    }
}
