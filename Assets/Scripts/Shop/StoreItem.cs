using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoreItem
{
    public ItemData data;
    

    public void Use(GameModify mod)
    {
        data.script.Use(mod);
    }
}

[System.Serializable]
public struct ItemData
{
    #if UNITY_EDITOR
    [HideInInspector] public bool foldout;
#endif

    public ItemScript script;

    public ItemType type;
    
    public string itemName;
    public string id;
    public string desc;
    public int cost;

    public Prerequisite prerequisite;
    public string preName;
    public int preValue;

    public Sprite storeIcon;
    public Sprite smallIcon;
}

[System.Serializable]
public enum Prerequisite
{
    Feat,
    Item
}

[System.Serializable]
public enum ItemType
{
    PlayerMod,
    EnemyMod,
    SpawnMod,
    Silly
}