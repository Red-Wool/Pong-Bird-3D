using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiLayerDrill : BossTemplate
{
    public GameObject firstLayer;
    public GameObject secondLayer;
    public GameObject finalLayer;



    /*Phase 1
    public ParticleSystem sprayAttack;
    public ParticleSystem spiralAttack;
    public ParticleSystem shootAttack;
    public ParticleSystem tunnelAttack;

    //Phase 2
    public ParticleSystem topsyAttack;
    public ParticleSystem topsyTunnelAttack;

    //Phase 3 
    public ParticleSystem ringAttack;
    public ParticleSystem expandAttack;
    */


    public override void Reset(Vector3 pos, GameObject t)
    {
        base.Reset(pos, t);

        firstLayer.transform.localPosition = Vector3.zero;
        secondLayer.transform.localPosition = Vector3.zero;
        finalLayer.transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        switch (phase)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
        }       
    }

    public override void Damage()
    {
        base.Damage();
        switch (phase)
        {
            case 1:
                firstLayer.SetActive(false);
                break;
            case 2:
                secondLayer.SetActive(false);
                break;
        }
    }
}
