﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item DataBase", menuName = "Inventory System/Items/DataBase")]
public class ItemObjectDataBase : ScriptableObject
{  
    public ItemObject[] itemObjects;

    public void OnValidate()
    {
        for (int i = 0; i < itemObjects.Length; ++i)
        {
            itemObjects[i].data.id = i;
        }
    }
}
