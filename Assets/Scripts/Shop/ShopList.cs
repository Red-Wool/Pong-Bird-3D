using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Shop Items")]
public class ShopList : ScriptableObject
{
    public List<StoreItem> items;
}

public struct StoreObject
{
    public StoreItem item;
}