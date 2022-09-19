using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShopCreator : EditorWindow
{
    ShopList shop;
    Vector2 scroll;

    [MenuItem("Tools/Shop Creator")] //Method toBe Called when going to the tools and stuff
    public static void ShowWindow()
    {
        GetWindow(typeof(ShopCreator)); //GetWindow is inherited from EditorWindow class;
    }

    private void OnGUI()
    {
        shop = (ShopList)EditorGUILayout.ObjectField("Shop Item", shop, typeof(ShopList), false);

        GUILayout.Label("Shop Items", EditorStyles.boldLabel);
        if (GUILayout.Button("Add Object"))
        {
            shop.items.Add(new StoreItem() { data = new ItemData() {itemName = "Default" } }) ;
            //EditorUtility.SetDirty(this);
        }


        if (shop)
        {
            scroll = EditorGUILayout.BeginScrollView(scroll);
            for (int i = 0; i < shop.items.Count; i++)
            {
                ItemData item = shop.items[i].data;

                item.foldout = EditorGUILayout.BeginFoldoutHeaderGroup(item.foldout, item.itemName);
                
                if (item.foldout)
                {
                   

                    item = DisplayShopItem(item);
                }

                shop.items[i].data = item;
                EditorGUILayout.EndFoldoutHeaderGroup();
                GUILayout.Space(15);
            }
            EditorGUILayout.EndScrollView();
        }
    }

    public ItemData DisplayShopItem(ItemData item)
    {
        GUILayout.Label(item.itemName, EditorStyles.boldLabel);
        //Type
        GUILayout.Space(15);
        item.type = (ItemType)EditorGUILayout.EnumPopup("Item Type", item.type);

        GUILayout.Space(15);
        //Item Name

        item.itemName = EditorGUILayout.TextField("Item Name", item.itemName);
        item.id = EditorGUILayout.TextField("Item ID", item.id);

        GUILayout.Space(15);
        item.desc = EditorGUILayout.TextField("Description", item.desc);
        item.cost = EditorGUILayout.IntField("Cost", item.cost);

        GUILayout.Space(15);
        //Prerequisite
        item.prerequisite = (Prerequisite)EditorGUILayout.EnumPopup("Prerequisite", item.prerequisite);

        EditorGUILayout.BeginHorizontal();

        item.preName = EditorGUILayout.TextField("Pre Name", item.preName);
        item.preValue = EditorGUILayout.IntField("Pre Value", item.preValue);

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(15);
        //Icon
        EditorGUILayout.BeginHorizontal();
        item.storeIcon = (Sprite)EditorGUILayout.ObjectField("Store Icon", item.storeIcon, typeof(Sprite), false);
        item.smallIcon = (Sprite)EditorGUILayout.ObjectField("Small Icon", item.smallIcon, typeof(Sprite), false);
        EditorGUILayout.EndHorizontal();

        return item;
    }
}
