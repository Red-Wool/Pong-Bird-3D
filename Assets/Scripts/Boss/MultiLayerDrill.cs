using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiLayerDrill : BossTemplate
{
    public GameObject firstLayer;
    public GameObject secondLayer;
    public GameObject finalLayer;

    public override void Reset(Vector3 pos, GameObject t)
    {
        base.Reset(pos, t);

        firstLayer.transform.localPosition = Vector3.zero;
        secondLayer.transform.localPosition = Vector3.zero;
        finalLayer.transform.localPosition = Vector3.zero;
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
