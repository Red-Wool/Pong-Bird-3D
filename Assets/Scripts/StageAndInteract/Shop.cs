using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : InteractObject
{
    [SerializeField] private PlayerMove player;
    [SerializeField] private GameObject shopMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact(PlayerMove play)
    {
        player = play;
        
        ActivateMenu(true);
        Debug.Log("Shopping time");
    }

    public void ActivateMenu(bool flag)
    {
        if (flag)
        {
            player.Stop();
            shopMenu.SetActive(true);
        }
        else
        {
            player.Resume();
            shopMenu.SetActive(false);
        }
        
    }
}
