using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemScript : ScriptableObject
{
    public virtual void Use(GameModify mod)
    {
        return;
    }
}

/*Template
 * [CreateAssetMenu()]
 * 
 * */
