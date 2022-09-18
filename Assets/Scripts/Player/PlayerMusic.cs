using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMusic : MonoBehaviour
{
    private PlayerMove move;

    [SerializeField] private AudioSource flight;
    [SerializeField] private float flightSmInSpd;
    [SerializeField] private float flightSmOutSpd;

    [Space(10)]
    [SerializeField] private AudioSource groundJump;
    [SerializeField] private AudioSource threeJump;
    [SerializeField] private AudioSource twoJump;
    [SerializeField] private AudioSource oneJump;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<PlayerMove>();
    }

    

    // Update is called once per frame
    void Update()
    {

        flight.volume = Mathf.Lerp(flight.volume, (move.IsDash) ? 1 : 0, (move.IsDash ? flightSmInSpd : flightSmOutSpd) * Time.deltaTime);
    }

    public void JumpSound(int v)
    {


        switch (v)
        {
            case 3:
                threeJump.Play();
                break;
            case 2:
                twoJump.Play();
                break;
            case 1:
                oneJump.Play();
                break;
            case 0:
                groundJump.Play();
                break;

        }
    }
}
